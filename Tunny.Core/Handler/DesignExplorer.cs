using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

using Python.Runtime;

using Tunny.Core.Util;

namespace Tunny.Core.Handler
{
    public class DesignExplorer
    {
        private bool _hasImage;
        private readonly string _targetStudyName;
        private readonly Settings.Storage _storage;

        public DesignExplorer(string text, Settings.Storage storage)
        {
            TLog.MethodStart();
            _targetStudyName = text;
            _storage = storage;
        }

        public void Run()
        {
            TLog.MethodStart();
            OutputResultCsv();
            KillExistTunnyServerProcess();
            int port = FindAvailablePort(8081);

            var server = new Process();
            string path = Path.Combine(TEnvVariables.DesignExplorerPath, "TunnyDEServer.exe");
            server.StartInfo.FileName = path;
            server.StartInfo.Arguments = port.ToString(CultureInfo.InvariantCulture);
            server.StartInfo.WorkingDirectory = TEnvVariables.DesignExplorerPath;
            server.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            server.StartInfo.UseShellExecute = true;
            server.Start();

            var client = new Process();
            client.StartInfo.FileName = $"http://127.0.0.1:{port}/index.html";
            client.StartInfo.UseShellExecute = true;
            client.Start();
        }

        private void OutputResultCsv()
        {
            TLog.MethodStart();
            string envPath = Path.Combine(TEnvVariables.TunnyEnvPath, "python", @"python310.dll");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
            if (PythonEngine.IsInitialized)
            {
                PythonEngine.Shutdown();
                TLog.Warning("PythonEngine is unintentionally initialized and therefore shut it down.");
            }
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                PyModule ps = Py.CreateScope();
                ps.Exec(
"def export_fish_csv(storage, target_study_name, output_path):\n" +
"    import optuna\n" +
"    import csv\n" +
"    import json\n" +

"    study = optuna.load_study(storage=storage, study_name=target_study_name)\n" +
"    s_id = (str)(study._study_id)\n" +
"    trial = study.get_trials(deepcopy=False, states=[optuna.trial.TrialState.COMPLETE])\n" +
"    metric_names = study.metric_names\n" +
"    param_keys = list(trial[0].params.keys())\n" +
"    artifact_path = 'http://127.0.0.1:8080/artifacts/' + s_id + '/'\n" +
"    has_img = False\n" +

"    label = []\n" +

"    for key in param_keys:\n" +
"        label.append('in:' + key)\n" +

"    if metric_names is not None:\n" +
"        for name in metric_names:\n" +
"            label.append('out:' + name)\n" +
"    for attr in trial[0].system_attrs:\n" +
"        if attr.startswith('artifacts:'):\n" +
"            artifact_value = trial[0].system_attrs[attr]\n" +
"            j = json.loads(artifact_value)\n" +
"            if j['mimetype'] == 'image/png':\n" +
"                label.append('img')\n" +
"                has_img = True\n" +

"    with open(output_path + '/fish.csv', 'w', newline='') as f:\n" +
"        writer = csv.writer(f)\n" +
"        writer.writerow(label)\n" +

"        for t in trial:\n" +
"            t_id = (str)(t.number)\n" +
"            row = []\n" +
"            for key in param_keys:\n" +
"                row.append(t.params[key])\n" +

"            for v in t.values:\n" +
"                row.append(v)\n" +

"            for attr in t.system_attrs:\n" +
"                if attr.startswith('artifacts:'):\n" +
"                    artifact_value = t.system_attrs[attr]\n" +
"                    j = json.loads(artifact_value)\n" +
"                    if j['mimetype'] == 'image/png':\n" +
"                        row.append(artifact_path + t_id + '/' + j['artifact_id'])\n" +
"                        break\n" +

"            writer.writerow(row)\n" +
"    data = {\n" +
"        'studyInfo': {\n" +
"            'name': study.study_name,\n" +
"            'date': 'Tunny result for DesignExplorer'\n" +
"        },\n" +
"        'dimScales': {},\n" +
"        'dimTicks': {},\n" +
"        'dimMark': {}\n" +
"    }\n" +

"    with open(output_path + '/settings.json', 'w') as file:\n" +
"        json.dump(data, file, indent=4)\n" +

"    return has_img\n"
                );
                dynamic storage = _storage.CreateNewOptunaStorage(false);
                dynamic func = ps.Get("export_fish_csv");
                string outputPath = Path.Combine(TEnvVariables.DesignExplorerPath, "design_explorer_data");
                _hasImage = func(storage, _targetStudyName, outputPath);
            }
        }

        private static bool KillExistTunnyServerProcess()
        {
            TLog.MethodStart();
            int killCount = 0;
            Process[] server = Process.GetProcessesByName("TunnyDEServer");
            if (server.Length > 0)
            {
                foreach (Process p in server)
                {
                    p.Kill();
                    killCount++;
                }
            }
            return killCount > 0;
        }

        private static int FindAvailablePort(int startPort)
        {
            TLog.MethodStart();
            int port = startPort;
            while (IsPortInUse(port))
            {
                port++;
            }
            return port;
        }

        private static bool IsPortInUse(int port)
        {
            TLog.MethodStart();
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endpoint in tcpConnInfoArray)
            {
                if (endpoint.Port == port)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
