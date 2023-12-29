using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Python.Runtime;

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
            int samplerType = Settings.Optimize.SelectSampler;
            int nTrials = Settings.Optimize.NumberOfTrials;
            double timeout = Settings.Optimize.Timeout <= 0 ? -1 : Settings.Optimize.Timeout;
            string[] directions = SetDirectionValues(Objective.Length);

            PythonEngine.Initialize();
            IntPtr allowThreadsPtr = PythonEngine.BeginAllowThreads();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic sampler = SetSamplerSettings(samplerType, optuna, HasConstraints);
                dynamic storage = Settings.Storage.CreateNewOptunaStorage(false);
                dynamic artifactBackend = Settings.Storage.CreateNewOptunaArtifactBackend(false);

                double[] xTest = null;
                TrialGrasshopperItems result = null;

                if (CheckExistStudyParameter(Objective.Length, optuna, storage))
                {
                    dynamic study;
                    switch (Objective.HumanInTheLoopType)
                    {
                        case HumanInTheLoopType.HumanSliderInput:
                            HumanSliderInputOptimization(nTrials, timeout, directions, sampler, storage, artifactBackend, out xTest, out result, out study);
                            break;
                        case HumanInTheLoopType.Preferential:
                            PreferentialOptimization(nTrials, storage, artifactBackend, out xTest, out result, out study);
                            break;
                        default:
                            NormalOptimization(nTrials, timeout, directions, sampler, storage, artifactBackend, out xTest, out result, out study);
                            break;
                    }
                    SetResultValues(Objective.Length, study, xTest);
                }
            }
            PythonEngine.EndAllowThreads(allowThreadsPtr);
            PythonEngine.Shutdown();
        }

        private void PreferentialOptimization(int nTrials, dynamic storage, dynamic artifactBackend, out double[] xTest, out TrialGrasshopperItems result, out dynamic study)
        {
            var preferentialOpt = new HumanInTheLoop.Preferential(Path.GetDirectoryName(Settings.Storage.Path));
            if (Objective.Length > 1)
            {
                TunnyMessageBox.Show("Human-in-the-Loop Preferential only supports single objective optimization. Optimization is run without considering constraints.", "Tunny");
            }
            // TODO: nBatch should be set by user
            int nBatch = 6;
            string[] objNickName = Objective.GetNickNames();
            study = preferentialOpt.CreateStudy(nBatch, Settings.StudyName, storage, objNickName[0]);
            var optInfo = new OptimizationHandlingInfo(nTrials, 0, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, NicknameToAttr(Variables.Select(v => v.NickName)), false);
            preferentialOpt.WakeOptunaDashboard(Settings.Storage);
            optInfo.Preferential = preferentialOpt;
            RunPreferentialOptimize(optInfo, nBatch, out xTest, out result);
        }

        private void NormalOptimization(int nTrials, double timeout, string[] directions, dynamic sampler, dynamic storage, dynamic artifactBackend, out double[] xTest, out TrialGrasshopperItems result, out dynamic study)
        {
            study = CreateStudy(directions, sampler, storage);
            string[] objNickName = Objective.GetNickNames();
            var optInfo = new OptimizationHandlingInfo(nTrials, timeout, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, NicknameToAttr(Variables.Select(v => v.NickName)));
            RunOptimize(optInfo, out xTest, out result);
        }

        private void HumanSliderInputOptimization(int nTrials, double timeout, string[] directions, dynamic sampler, dynamic storage, dynamic artifactBackend, out double[] xTest, out TrialGrasshopperItems result, out dynamic study)
        {
            // TODO: nBatch should be set by user
            int nBatch = 4;
            study = CreateStudy(directions, sampler, storage);
            string[] objNickName = Objective.GetNickNames();
            var optInfo = new OptimizationHandlingInfo(nTrials, timeout, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, NicknameToAttr(Variables.Select(v => v.NickName)));
            var humanSliderInput = new HumanInTheLoop.HumanSliderInput(Path.GetDirectoryName(Settings.Storage.Path));
            humanSliderInput.WakeOptunaDashboard(Settings.Storage);
            humanSliderInput.SetObjective(study, objNickName);
            humanSliderInput.SetWidgets(study, objNickName);
            optInfo.HumanSliderInput = humanSliderInput;
            RunHumanSidlerInputOptimize(optInfo, nBatch, out xTest, out result);
        }

        private static StringBuilder NicknameToAttr(IEnumerable<string> nicknames)
        {
            var name = new StringBuilder();
            foreach (string objName in nicknames)
            {
                name.Append(objName + ",");
            }
            name.Remove(name.Length - 1, 1);
            return name;
        }

        private dynamic CreateStudy(string[] directions, dynamic sampler, dynamic storage)
        {
            dynamic optuna = Py.Import("optuna");
            string studyName = Settings.StudyName == null || Settings.StudyName == "" ? "no-name-" + Guid.NewGuid().ToString("D") : Settings.StudyName;
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

        private void SetResultValues(int nObjective, dynamic study, double[] xTest)
        {
            if (nObjective == 1)
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
                XOpt = xTest;
            }
        }

        private void RunOptimize(OptimizationHandlingInfo optInfo, out double[] xTest, out TrialGrasshopperItems result)
        {
            xTest = new double[Variables.Count];
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
                result = RunSingleOptimizeStep(optInfo, xTest, trialNum, startTime);
                trialNum++;
            }

            SaveInMemoryStudy(optInfo.Storage);
        }

        private void RunPreferentialOptimize(OptimizationHandlingInfo optInfo, int nBatch, out double[] xTest, out TrialGrasshopperItems result)
        {
            xTest = new double[Variables.Count];
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
                result = RunSingleOptimizeStep(optInfo, xTest, trialNum, startTime);
                trialNum++;
            }

            SaveInMemoryStudy(optInfo.Storage);
        }

        private void RunHumanSidlerInputOptimize(OptimizationHandlingInfo optInfo, int nBatch, out double[] xTest, out TrialGrasshopperItems result)
        {
            xTest = new double[Variables.Count];
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
                result = RunSingleOptimizeStep(optInfo, xTest, trialNum, startTime);
                trialNum++;
            }

            SaveInMemoryStudy(optInfo.Storage);
        }

        private TrialGrasshopperItems RunSingleOptimizeStep(OptimizationHandlingInfo optInfo, double[] xTest, int trialNum, DateTime startTime)
        {
            dynamic trial;
            int progress = trialNum * 100 / optInfo.NTrials;
            var result = new TrialGrasshopperItems();

            int nullCount = 0;
            while (true)
            {
                if (optInfo.Preferential != null && !optInfo.Study.should_generate())
                {
                    System.Threading.Thread.Sleep(100);
                    continue;
                }

                trial = optInfo.Study.ask();

                for (int j = 0; j < Variables.Count; j++)
                {
                    xTest[j] = Variables[j].IsInteger
                    ? trial.suggest_int(Variables[j].NickName, Variables[j].LowerBond, Variables[j].UpperBond, step: Variables[j].Epsilon)
                    : trial.suggest_float(Variables[j].NickName, Variables[j].LowerBond, Variables[j].UpperBond, step: Variables[j].Epsilon);
                }

                ProgressState pState = SetProgressState(optInfo, xTest, trialNum, startTime);
                result = EvalFunc(pState, progress);
                if (optInfo.HumanSliderInput != null)
                {
                    optInfo.HumanSliderInput.SaveNote(optInfo.Study, trial, result.Objectives.Images);
                }
                else if (optInfo.Preferential != null && result.Objectives.Images.Length == 1)
                {
                    optInfo.Preferential.UploadArtifact(optInfo.Study, trial, result.Objectives.Images[0]);
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
                }
                else if (optInfo.HumanSliderInput == null && optInfo.Preferential == null)
                {
                    optInfo.Study.tell(trial, result.Objectives.Numbers);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
            finally
            {
                RunGC(result);
            }

            return result;
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

        private ProgressState SetProgressState(OptimizationHandlingInfo optSet, double[] xTest, int trialNum, DateTime startTime)
        {
            ComputeBestValues(optSet.Study, trialNum, out double[][] bestValues, out double hypervolumeRatio);
            return new ProgressState
            {
                TrialNumber = trialNum,
                ObjectiveNum = Objective.Length,
                BestValues = bestValues,
                Values = xTest.Select(v => (decimal)v).ToList(),
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

        private void SetStudyUserAttr(dynamic study, StringBuilder variableName, bool setMetricNames = true)
        {
            study.set_user_attr("variable_names", variableName.ToString());
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

        private dynamic SetSamplerSettings(int samplerType, dynamic optuna, bool hasConstraints)
        {
            dynamic sampler;
            switch (samplerType)
            {
                case 0:
                case 7:
                    sampler = Sampler.TPE(optuna, Settings, hasConstraints);
                    break;
                case 1:
                    sampler = Sampler.BoTorch(optuna, Settings, hasConstraints);
                    break;
                case 2:
                    sampler = Sampler.NSGAII(optuna, Settings, hasConstraints);
                    break;
                case 3:
                    sampler = Sampler.NSGAIII(optuna, Settings, hasConstraints);
                    break;
                case 4:
                    sampler = Sampler.CmaEs(optuna, Settings);
                    break;
                case 5:
                    sampler = Sampler.QMC(optuna, Settings);
                    break;
                case 6:
                    sampler = Sampler.Random(optuna, Settings);
                    break;
                default:
                    throw new ArgumentException("Unknown sampler type");
            }
            if (samplerType > 4 && hasConstraints)
            {
                TunnyMessageBox.Show("Only TPE, GP and NSGA support constraints. Optimization is run without considering constraints.", "Tunny");
            }
            return sampler;
        }
    }
}
