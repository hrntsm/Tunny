using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Rhino.Geometry;

using Tunny.Component;
using Tunny.Solver;
using Tunny.Type;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Optimization
{
    internal static class RestoreLoop
    {
        private static BackgroundWorker s_worker;
        private static TunnyComponent s_component;
        public static string StudyName;
        public static string[] NickNames;
        public static int[] Indices;
        public static string Mode;

        internal static void Run(object sender, DoWorkEventArgs e)
        {
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as TunnyComponent;

            var fishes = new List<Fish>();

            var optunaSolver = new Optuna(s_component.GhInOut.ComponentFolder);
            ModelResult[] modelResult = optunaSolver.GetModelResult(Indices, StudyName);
            if (modelResult.Length == 0)
            {
                TunnyMessageBox.Show("There are no restore models. Please check study name.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (Mode)
            {
                case "Restore":
                    for (int i = 0; i < modelResult.Length; i++)
                    {
                        SetResultToFish(fishes, modelResult[i], NickNames);
                        s_worker.ReportProgress(i * 100 / modelResult.Length);
                    }
                    break;
                case "Reflect":
                    if (modelResult.Length > 1)
                    {
                        TunnyMessageBox.Show(
                            "You input multi restore model numbers, but this function only reflect variables to slider or genepool to first one.",
                            "Tunny"
                        );
                    }
                    SetResultToFish(fishes, modelResult[0], NickNames);
                    s_worker.ReportProgress(100);
                    break;
            }

            s_component.Fishes = fishes.ToArray();
            s_worker.ReportProgress(100);

            if (s_worker != null)
            {
                s_worker.CancelAsync();
            }
            TunnyMessageBox.Show("Restore completed successfully.", "Tunny");
        }

        private static void SetResultToFish(ICollection<Fish> fishes, ModelResult model, IEnumerable<string> nickname)
        {
            fishes.Add(new Fish
            {
                ModelNumber = model.Number,
                Variables = SetVariables(model, nickname),
                Objectives = SetObjectives(model),
                Attributes = SetAttributes(model),
            });
        }

        private static Dictionary<string, double> SetVariables(ModelResult model, IEnumerable<string> nickNames)
        {
            var variables = new Dictionary<string, double>();
            foreach (string name in nickNames)
            {
                foreach (KeyValuePair<string, double> variable in model.Variables.Where(obj => obj.Key == name))
                {
                    variables.Add(variable.Key, variable.Value);
                }
            }
            return variables;
        }

        private static Dictionary<string, double> SetObjectives(ModelResult model)
        {
            string[] nickNames = s_component.GhInOut.Objectives.Select(x => x.NickName).ToArray();
            var objectives = new Dictionary<string, double>();
            if (model.Objectives == null)
            {
                return null;
            }
            for (int i = 0; i < model.Objectives.Length; i++)
            {
                objectives.Add(nickNames[i], model.Objectives[i]);
            }
            return objectives;
        }

        private static Dictionary<string, object> SetAttributes(ModelResult model)
        {
            var attribute = new Dictionary<string, object>();
            foreach (KeyValuePair<string, List<string>> attr in model.Attributes)
            {
                if (attr.Key == "Geometry")
                {
                    var geometries = new List<GeometryBase>();
                    foreach (string json in attr.Value)
                    {
                        geometries.Add(Rhino.Runtime.CommonObject.FromJSON(json) as GeometryBase);
                    }
                    attribute.Add(attr.Key, geometries);
                }
                else
                {
                    attribute.Add(attr.Key, attr.Value);
                }
            }
            return attribute;
        }
    }
}
