using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Python.Runtime;

using Serilog;

using Tunny.Enum;
using Tunny.Handler;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.Settings;
using Tunny.Storage;
using Tunny.Type;
using Tunny.UI;

namespace Tunny.Solver
{
    public class Algorithm
    {
        public double[] XOpt { get; private set; }
        public EndState EndState { get; private set; }

        private List<Variable> Variables { get; }
        private bool HasConstraints { get; }
        private Objective Objective { get; }
        private TunnySettings Settings { get; }
        private Func<ProgressState, int, TrialGrasshopperItems> EvalFunc { get; }
        private Dictionary<string, FishEgg> FishEgg { get; }

        public Algorithm(
            List<Variable> variables, bool hasConstraint, Objective objective, Dictionary<string, FishEgg> fishEgg,
            TunnySettings settings,
            Func<ProgressState, int, TrialGrasshopperItems> evalFunc)
        {
            Variables = variables;
            HasConstraints = hasConstraint;
            Objective = objective;
            FishEgg = fishEgg;
            Settings = settings;
            EvalFunc = evalFunc;
        }

        public void Solve()
        {
            EndState = EndState.Error;
            OptimizeLoop.IsForcedStopOptimize = false;
            SamplerType samplerType = Settings.Optimize.SelectSampler;
            int nTrials = Settings.Optimize.NumberOfTrials;
            double timeout = Settings.Optimize.Timeout <= 0 ? -1 : Settings.Optimize.Timeout;
            string[] directions = SetDirectionValues(Objective.Length);
            Log.Information($"Optimization started with {nTrials} trials and {timeout} seconds timeout and {samplerType} sampler.");

            PythonEngine.Initialize();
            IntPtr allowThreadsPtr = PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic sampler = SetSamplerSettings(samplerType, optuna, HasConstraints);
                dynamic storage = Settings.Storage.CreateNewOptunaStorage(false);
                dynamic artifactBackend = Settings.Storage.CreateNewOptunaArtifactBackend(false);

                double[] parameter = null;
                TrialGrasshopperItems result = null;

                if (CheckExistStudyParameter(Objective.Length, optuna, storage))
                {
                    dynamic study;
                    switch (Objective.HumanInTheLoopType)
                    {
                        case HumanInTheLoopType.HumanSliderInput:
                            HumanSliderInputOptimization(nTrials, timeout, directions, sampler, storage, artifactBackend, out parameter, out result, out study);
                            break;
                        case HumanInTheLoopType.Preferential:
                            PreferentialOptimization(nTrials, storage, artifactBackend, out parameter, out result, out study);
                            break;
                        default:
                            NormalOptimization(nTrials, timeout, directions, sampler, storage, artifactBackend, out parameter, out result, out study);
                            break;
                    }
                    SetResultValues(Objective.Length, study, parameter);
                }
            }
            PythonEngine.EndAllowThreads(allowThreadsPtr);
            PythonEngine.Shutdown();
        }

        private void PreferentialOptimization(int nBatch, dynamic storage, dynamic artifactBackend, out double[] parameter, out TrialGrasshopperItems result, out dynamic study)
        {
            if (Settings.Storage.Type != StorageType.Sqlite)
            {
                TunnyMessageBox.Show("Human-in-the-Loop Preferential only supports SQlite storage. ", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException("Human-in-the-Loop Preferential only supports SQlite storage. ");
            }
            var preferentialOpt = new HumanInTheLoop.Preferential(Path.GetDirectoryName(Settings.Storage.Path));
            if (Objective.Length > 1)
            {
                TunnyMessageBox.Show("Human-in-the-Loop Preferential only supports single objective optimization. Optimization is run without considering constraints.", "Tunny");
            }
            string[] objNickName = Objective.GetNickNames();
            study = preferentialOpt.CreateStudy(nBatch, Settings.StudyName, storage, objNickName[0]);
            var optInfo = new OptimizationHandlingInfo(int.MaxValue, 0, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, NicknameToPyList(Variables.Select(v => v.NickName)), false);
            preferentialOpt.WakeOptunaDashboard(Settings.Storage);
            optInfo.Preferential = preferentialOpt;
            RunPreferentialOptimize(optInfo, nBatch, out parameter, out result);
        }

        private void NormalOptimization(int nTrials, double timeout, string[] directions, dynamic sampler, dynamic storage, dynamic artifactBackend, out double[] parameter, out TrialGrasshopperItems result, out dynamic study)
        {
            study = CreateStudy(directions, sampler, storage);
            string[] objNickName = Objective.GetNickNames();
            var optInfo = new OptimizationHandlingInfo(nTrials, timeout, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, NicknameToPyList(Variables.Select(v => v.NickName)));
            RunOptimize(optInfo, out parameter, out result);
        }

        private void HumanSliderInputOptimization(int nBatch, double timeout, string[] directions, dynamic sampler, dynamic storage, dynamic artifactBackend, out double[] parameter, out TrialGrasshopperItems result, out dynamic study)
        {
            study = CreateStudy(directions, sampler, storage);
            string[] objNickName = Objective.GetNickNames();
            var optInfo = new OptimizationHandlingInfo(int.MaxValue, timeout, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, NicknameToPyList(Variables.Select(v => v.NickName)));
            var humanSliderInput = new HumanInTheLoop.HumanSliderInput(Path.GetDirectoryName(Settings.Storage.Path));
            humanSliderInput.WakeOptunaDashboard(Settings.Storage);
            humanSliderInput.SetObjective(study, objNickName);
            humanSliderInput.SetWidgets(study, objNickName);
            optInfo.HumanSliderInput = humanSliderInput;
            RunHumanSidlerInputOptimize(optInfo, nBatch, out parameter, out result);
        }

        private static PyList NicknameToPyList(IEnumerable<string> nicknames)
        {
            var name = new PyList();
            foreach (string nickname in nicknames)
            {
                name.Append(new PyString(nickname));
            }
            return name;
        }

        private dynamic CreateStudy(string[] directions, dynamic sampler, dynamic storage)
        {
            dynamic optuna = Py.Import("optuna");
            if (Settings.StudyName == null || Settings.StudyName == "")
            {
                Settings.StudyName = "no-name-" + Guid.NewGuid().ToString("D");
            }
            string studyName = Settings.StudyName;
            return optuna.create_study(
                sampler: sampler,
                directions: directions,
                storage: storage,
                study_name: studyName,
                load_if_exists: Settings.Optimize.ContinueStudy
            );
        }

        private static string[] SetDirectionValues(int nObjective)
        {
            string[] directions = new string[nObjective];
            for (int i = 0; i < nObjective; i++)
            {
                directions[i] = "minimize";
            }

            return directions;
        }

        private bool CheckExistStudyParameter(int nObjective, dynamic optuna, dynamic storage)
        {
            PyList studySummaries = optuna.get_all_study_summaries(storage);
            var studySummaryDict = new Dictionary<string, int>();

            foreach (dynamic pyObj in studySummaries)
            {
                studySummaryDict.Add((string)pyObj.study_name, (int)pyObj.directions.__len__());
            }

            return !studySummaryDict.ContainsKey(Settings.StudyName) || CheckDirections(nObjective, studySummaryDict);
        }

        private bool CheckDirections(int nObjective, IReadOnlyDictionary<string, int> directions)
        {
            if (!Settings.Optimize.ContinueStudy)
            {
                EndState = EndState.UseExitStudyWithoutLoading;
                return false;
            }
            else if (directions[Settings.StudyName] == nObjective)
            {
                return true;
            }
            else
            {
                EndState = EndState.DirectionNumNotMatch;
                return false;
            }
        }

        private void SetResultValues(int nObjective, dynamic study, double[] parameter)
        {
            if (nObjective == 1 && Objective.HumanInTheLoopType != HumanInTheLoopType.Preferential)
            {
                double[] values = (double[])study.best_params.values();
                string[] keys = (string[])study.best_params.keys();
                double[] opt = new double[Variables.Count];

                for (int i = 0; i < Variables.Count; i++)
                {
                    for (int j = 0; j < keys.Length; j++)
                    {
                        if (keys[j] == Variables[i].NickName)
                        {
                            opt[i] = values[j];
                        }
                    }
                }
                XOpt = opt;
            }
            else
            {
                XOpt = parameter;
            }
        }

        private void RunOptimize(OptimizationHandlingInfo optInfo, out double[] parameter, out TrialGrasshopperItems result)
        {
            parameter = new double[Variables.Count];
            result = new TrialGrasshopperItems();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optInfo.Study, optInfo.EnqueueItems);

            while (true)
            {
                if (result == null || CheckOptimizeComplete(optInfo.NTrials, optInfo.Timeout, trialNum, startTime))
                {
                    break;
                }
                result = RunSingleOptimizeStep(optInfo, parameter, trialNum, startTime);
                trialNum++;
            }

            SaveInMemoryStudy(optInfo.Storage);
        }

        private void RunPreferentialOptimize(OptimizationHandlingInfo optInfo, int nBatch, out double[] parameter, out TrialGrasshopperItems result)
        {
            parameter = new double[Variables.Count];
            result = new TrialGrasshopperItems();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optInfo.Study, optInfo.EnqueueItems);

            while (true)
            {
                if (result == null || CheckOptimizeComplete(optInfo.NTrials, optInfo.Timeout, trialNum, startTime))
                {
                    break;
                }
                if (HumanInTheLoop.Preferential.GetRunningTrialNumber(optInfo.Study) >= nBatch)
                {
                    continue;
                }
                result = RunSingleOptimizeStep(optInfo, parameter, trialNum, startTime);
                trialNum++;
            }

            SaveInMemoryStudy(optInfo.Storage);
        }

        private void RunHumanSidlerInputOptimize(OptimizationHandlingInfo optInfo, int nBatch, out double[] parameter, out TrialGrasshopperItems result)
        {
            parameter = new double[Variables.Count];
            result = new TrialGrasshopperItems();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optInfo.Study, optInfo.EnqueueItems);

            while (true)
            {
                if (result == null || CheckOptimizeComplete(optInfo.NTrials, optInfo.Timeout, trialNum, startTime))
                {
                    break;
                }
                if (HumanInTheLoop.HumanSliderInput.GetRunningTrialNumber(optInfo.Study) >= nBatch)
                {
                    continue;
                }
                result = RunSingleOptimizeStep(optInfo, parameter, trialNum, startTime);
                trialNum++;
            }

            SaveInMemoryStudy(optInfo.Storage);
        }

        private TrialGrasshopperItems RunSingleOptimizeStep(OptimizationHandlingInfo optInfo, double[] parameter, int trialNum, DateTime startTime)
        {
            dynamic trial;
            int progress = trialNum * 100 / optInfo.NTrials;
            var result = new TrialGrasshopperItems();

            int nullCount = 0;
            while (true)
            {
                if (optInfo.Preferential != null && !optInfo.Study.should_generate() && !OptimizeLoop.IsForcedStopOptimize)
                {
                    Thread.Sleep(100);
                    continue;
                }

                trial = optInfo.Study.ask();

                for (int j = 0; j < Variables.Count; j++)
                {
                    parameter[j] = Variables[j].IsInteger
                    ? trial.suggest_int(Variables[j].NickName, Variables[j].LowerBond, Variables[j].UpperBond, step: Variables[j].Epsilon)
                    : trial.suggest_float(Variables[j].NickName, Variables[j].LowerBond, Variables[j].UpperBond, step: Variables[j].Epsilon);
                }

                ProgressState pState = SetProgressState(optInfo, parameter, trialNum, startTime);
                result = EvalFunc(pState, progress);
                if (optInfo.HumanSliderInput != null)
                {
                    optInfo.HumanSliderInput.SaveNote(optInfo.Study, trial, result.Objectives.Images);
                }
                else if (optInfo.Preferential != null && result.Objectives.Images.Length == 1)
                {
                    optInfo.Preferential.UploadArtifact(trial, result.Objectives.Images[0]);
                }

                if (nullCount >= 10)
                {
                    return TenTimesNullResultErrorMessage();
                }
                else if (result.Objectives.Numbers.Contains(double.NaN))
                {
                    nullCount++;
                }
                else
                {
                    break;
                }
            }

            SetTrialUserAttr(result, trial, optInfo);
            try
            {
                if (result.Artifacts.Count() > 0)
                {
                    UploadArtifacts(result.Artifacts, optInfo.ArtifactBackend, trial);
                }
                if (result.Attribute.TryGetValue("IsFAIL", out List<string> isFail) && isFail.Contains("True"))
                {
                    dynamic optuna = Py.Import("optuna");
                    optInfo.Study.tell(trial, state: optuna.trial.TrialState.FAIL);
                    Log.Warning($"Trial {trialNum} failed.");
                }
                else if (optInfo.HumanSliderInput == null && optInfo.Preferential == null)
                {
                    optInfo.Study.tell(trial, result.Objectives.Numbers);
                    SetTrialResultLog(trialNum, result, optInfo, parameter);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
            finally
            {
                RunGC(result);
            }

            return result;
        }

        private void SetTrialResultLog(int trialNum, TrialGrasshopperItems result, OptimizationHandlingInfo optInfo, double[] parameter)
        {
            var sb = new StringBuilder();
            sb.Append("Trial " + trialNum + " finished with values: {");
            foreach (string value in optInfo.ObjectiveNames.Zip(result.Objectives.Numbers, (name, value) => "'" + name + "': " + value))
            {
                sb.Append(value + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("} and parameters: {");
            foreach (string value in Variables.Zip(parameter, (v, value) => "'" + v.NickName + "': " + value))
            {
                sb.Append(value + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("}.");
            Log.Information(sb.ToString());
        }

        private void UploadArtifacts(Artifact artifacts, dynamic artifactBackend, dynamic trial)
        {
            string dir = Path.GetDirectoryName(Settings.Storage.Path);
            string fileName = $"artifact_trial_{trial.number}";
            string basePath = Path.Combine(dir, fileName);
            artifacts.SaveAllArtifacts(basePath);
            List<string> artifactPath = artifacts.ArtifactPaths;

            dynamic optuna = Py.Import("optuna");
            foreach (string path in artifactPath)
            {
                optuna.artifacts.upload_artifact(trial, path, artifactBackend);
                if (Path.GetFileNameWithoutExtension(path).Contains(fileName))
                {
                    File.Delete(path);
                }
            }
        }

        private static TrialGrasshopperItems TenTimesNullResultErrorMessage()
        {
            TunnyMessageBox.Show(
                "The objective function returned NaN 10 times in a row. Tunny terminates the optimization. Please check the objective function.",
                "Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return null;
        }

        private bool CheckOptimizeComplete(int nTrials, double timeout, int trialNum, DateTime startTime)
        {
            bool isOptimizeCompleted = false;
            if (trialNum >= nTrials)
            {
                EndState = EndState.AllTrialCompleted;
                isOptimizeCompleted = true;
            }
            else if (timeout > 0 && (DateTime.Now - startTime).TotalSeconds >= timeout)
            {
                EndState = EndState.Timeout;
                isOptimizeCompleted = true;
            }
            else if (OptimizeLoop.IsForcedStopOptimize)
            {
                EndState = EndState.StoppedByUser;
                OptimizeLoop.IsForcedStopOptimize = false;
                isOptimizeCompleted = true;
            }

            return isOptimizeCompleted;
        }

        private ProgressState SetProgressState(OptimizationHandlingInfo optSet, double[] parameter, int trialNum, DateTime startTime)
        {
            ComputeBestValues(optSet.Study, trialNum, out double[][] bestValues, out double hypervolumeRatio);
            return new ProgressState
            {
                TrialNumber = trialNum,
                ObjectiveNum = Objective.Length,
                BestValues = bestValues,
                Values = parameter.Select(v => (decimal)v).ToList(),
                HypervolumeRatio = hypervolumeRatio,
                EstimatedTimeRemaining = optSet.Timeout <= 0
                    ? TimeSpan.FromSeconds((DateTime.Now - startTime).TotalSeconds * (optSet.NTrials - trialNum) / (trialNum + 1))
                    : TimeSpan.FromSeconds(optSet.Timeout - (DateTime.Now - startTime).TotalSeconds)
            };
        }

        private void ComputeBestValues(dynamic study, int trialNum, out double[][] bestValues, out double hypervolumeRatio)
        {
            if (Settings.Optimize.ShowRealtimeResult)
            {
                dynamic[] bestTrials = study.best_trials;
                bestValues = bestTrials.Select(t => (double[])t.values).ToArray();
                hypervolumeRatio = trialNum == 0 ? 0 : trialNum == 1 || Objective.Length == 1 ? 1 : Hypervolume.Compute2dHypervolumeRatio(study);
            }
            else
            {
                bestValues = null;
                hypervolumeRatio = 0;
            }
        }

        private void SaveInMemoryStudy(dynamic storage)
        {
            if (Settings.Storage.Type == StorageType.InMemory)
            {
                dynamic optuna = Py.Import("optuna");
                string studyName = Settings.StudyName;
                optuna.copy_study(from_study_name: studyName, to_study_name: studyName, from_storage: storage, to_storage: new StorageHandler().CreateNewStorage(false, Settings.Storage));
            }
        }

        private static dynamic EnqueueTrial(dynamic study, Dictionary<string, FishEgg> enqueueItems)
        {
            if (enqueueItems == null || enqueueItems.Count == 0)
            {
                return study;
            }

            for (int i = 0; i < enqueueItems.First().Value.Values.Count; i++)
            {
                var enqueueDict = new PyDict();
                foreach (KeyValuePair<string, FishEgg> enqueueItem in enqueueItems)
                {
                    enqueueDict.SetItem(new PyString(enqueueItem.Key), new PyFloat(enqueueItem.Value.Values[i]));
                }
                study.enqueue_trial(enqueueDict, skip_if_exists: true);
            }

            return study;
        }

        private void RunGC(TrialGrasshopperItems result)
        {
            GcAfterTrial gcAfterTrial = Settings.Optimize.GcAfterTrial;
            if ((gcAfterTrial != GcAfterTrial.Always) &&
                ((result.GeometryJson.Length <= 0) || (gcAfterTrial != GcAfterTrial.HasGeometry)))
            {
                return;
            }

            dynamic gc = Py.Import("gc");
            gc.collect();
        }

        private void SetStudyUserAttr(dynamic study, PyList variableName, bool setMetricNames = true)
        {
            study.set_user_attr("variable_names", variableName);
            study.set_user_attr("tunny_version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
            if (setMetricNames)
            {
                study.set_metric_names(Objective.GetPyListStyleNickname());
            }
        }

        private static void SetTrialUserAttr(TrialGrasshopperItems result, dynamic trial, OptimizationHandlingInfo optSet)
        {
            if (result.GeometryJson.Length != 0)
            {
                var pyJson = new PyList();
                foreach (string json in result.GeometryJson)
                {
                    pyJson.Append(new PyString(json));
                }
                trial.set_user_attr("Geometry", pyJson);
            }

            if (result.Attribute != null)
            {
                SetNonGeometricAttr(result, trial);
            }

            if (result.Objectives.Length != 0 && optSet.HumanSliderInput != null)
            {
                int imageCount = 0;
                for (int i = 0; i < result.Objectives.Length; i++)
                {
                    if (!optSet.ObjectiveNames[i].Contains("Human-in-the-Loop"))
                    {
                        var key = new PyString("result_" + optSet.ObjectiveNames[i]);
                        var value = new PyFloat(result.Objectives.Numbers[i - imageCount]);
                        trial.set_user_attr(key, value);
                    }
                    else
                    {
                        imageCount++;
                    }
                }
            }
        }

        private static void SetNonGeometricAttr(TrialGrasshopperItems result, dynamic trial)
        {
            foreach (KeyValuePair<string, List<string>> pair in result.Attribute)
            {
                var pyList = new PyList();
                if (pair.Key == "Constraint")
                {
                    foreach (string str in pair.Value)
                    {
                        pyList.Append(new PyFloat(double.Parse(str, CultureInfo.InvariantCulture)));
                    }
                }
                else
                {
                    foreach (string str in pair.Value)
                    {
                        pyList.Append(new PyString(str));
                    }
                }
                trial.set_user_attr(pair.Key, pyList);
            }
        }

        private dynamic SetSamplerSettings(SamplerType samplerType, dynamic optuna, bool hasConstraints)
        {
            dynamic sampler;
            switch (samplerType)
            {
                case SamplerType.TPE:
                    sampler = Sampler.TPE(optuna, Settings, hasConstraints);
                    break;
                case SamplerType.BoTorch:
                    sampler = Sampler.BoTorch(optuna, Settings, hasConstraints);
                    break;
                case SamplerType.NSGAII:
                    sampler = Sampler.NSGAII(optuna, Settings, hasConstraints);
                    break;
                case SamplerType.NSGAIII:
                    sampler = Sampler.NSGAIII(optuna, Settings, hasConstraints);
                    break;
                case SamplerType.CmaEs:
                    sampler = Sampler.CmaEs(optuna, Settings);
                    break;
                case SamplerType.QMC:
                    sampler = Sampler.QMC(optuna, Settings);
                    break;
                case SamplerType.Random:
                    sampler = Sampler.Random(optuna, Settings);
                    break;
                default:
                    throw new ArgumentException("Invalid sampler type.");
            }
            if ((int)samplerType > 4 && hasConstraints)
            {
                TunnyMessageBox.Show("Only TPE, GP and NSGA support constraints. Optimization is run without considering constraints.", "Tunny");
            }
            return sampler;
        }
    }
}
