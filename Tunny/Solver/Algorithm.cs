using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Python.Runtime;

using Tunny.Handler;
using Tunny.Settings;
using Tunny.Storage;
using Tunny.Type;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Algorithm
    {
        private List<Variable> Variables { get; set; }
        private bool HasConstraints { get; set; }
        private string[] ObjNickName { get; set; }
        private TunnySettings Settings { get; set; }
        private Func<ProgressState, int, EvaluatedGHResult> EvalFunc { get; set; }
        public double[] XOpt { get; private set; }
        private double[] FxOpt { get; set; }
        public EndState EndState { get; set; }
        public Dictionary<string, FishEgg> FishEgg { get; set; }

        public Algorithm(
            List<Variable> variables, bool hasConstraint, string[] objNickName, Dictionary<string, FishEgg> fishEgg,
            TunnySettings settings,
            Func<ProgressState, int, EvaluatedGHResult> evalFunc)
        {
            Variables = variables;
            HasConstraints = hasConstraint;
            ObjNickName = objNickName;
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
            int nObjective = Settings.Optimize.SelectSampler == 6 ? ObjNickName.Length + 1 : ObjNickName.Length;
            string[] directions = SetDirectionValues(nObjective);

            if (Settings.Optimize.SelectSampler == 6)
            {
                Settings.Storage.Type = StorageType.InMemory;
            }

            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic sampler = SetSamplerSettings(samplerType, optuna, HasConstraints);
                dynamic storage = Settings.Storage.CreateNewOptunaStorage(false);

                double[] xTest = null;
                EvaluatedGHResult result = null;

                if (CheckExistStudyParameter(nObjective, optuna, storage))
                {
                    dynamic study = CreateStudy(directions, sampler, storage);
                    var runOptimizeSettings = new RunOptimizeSettings(nTrials, timeout, study, storage, FishEgg, ObjNickName);
                    SetStudyUserAttr(study, NicknameToAttr(Variables.Select(v => v.NickName)), NicknameToAttr(ObjNickName));
                    if (Settings.Optimize.SelectSampler == 6)
                    {
                        var humanInTheLoop = new HumanInTheLoop(Path.GetDirectoryName(Settings.Storage.Path));
                        humanInTheLoop.StartDashboardServerOnBackground(storage);
                        humanInTheLoop.SetObjective(study, ObjNickName);
                        humanInTheLoop.SetWidgets(study, ObjNickName);
                        runOptimizeSettings.HumanInTheLoop = humanInTheLoop;
                        RunHumanInTheLoopOptimize(runOptimizeSettings, out xTest, out result);
                    }
                    else
                    {
                        RunOptimize(runOptimizeSettings, out xTest, out result);
                    }
                    SetResultValues(nObjective, study, xTest, result);
                }
            }
            PythonEngine.Shutdown();
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
            return optuna.create_study(
                sampler: sampler,
                directions: directions,
                storage: storage,
                study_name: Settings.StudyName,
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

        private bool CheckDirections(int nObjective, Dictionary<string, int> directions)
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

        private void SetResultValues(int nObjective, dynamic study, double[] xTest, EvaluatedGHResult result)
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
                FxOpt = new[] { (double)study.best_value };
            }
            else
            {
                XOpt = xTest;
                FxOpt = result.ObjectiveValues?.ToArray();
            }
        }

        private void RunOptimize(RunOptimizeSettings optSet, out double[] xTest, out EvaluatedGHResult result)
        {
            xTest = new double[Variables.Count];
            result = new EvaluatedGHResult();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optSet.Study, optSet.EnqueueItems);

            while (true)
            {
                if (CheckOptimizeComplete(optSet.NTrials, optSet.Timeout, trialNum, startTime))
                {
                    break;
                }
                result = RunSingleOptimizeStep(optSet, xTest, trialNum, startTime);
                trialNum++;
            }

            if (Settings.Storage.Type == StorageType.InMemory)
            {
                CopyInMemoryStudy(optSet.Storage);
            }
        }

        private void RunHumanInTheLoopOptimize(RunOptimizeSettings optSet, out double[] xTest, out EvaluatedGHResult result)
        {
            xTest = new double[Variables.Count];
            result = new EvaluatedGHResult();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optSet.Study, optSet.EnqueueItems);
            int nBatch = 3;

            while (true)
            {
                if (CheckOptimizeComplete(optSet.NTrials, optSet.Timeout, trialNum, startTime))
                {
                    break;
                }
                if (HumanInTheLoop.GetRunningTrialNumber(optSet.Study) >= nBatch)
                {
                    continue;
                }
                result = RunSingleOptimizeStep(optSet, xTest, trialNum, startTime);
                trialNum++;
            }

            if (Settings.Storage.Type == StorageType.InMemory)
            {
                CopyInMemoryStudy(optSet.Storage);
            }
        }

        private EvaluatedGHResult RunSingleOptimizeStep(RunOptimizeSettings optSet, double[] xTest, int trialNum, DateTime startTime)
        {
            int progress = trialNum * 100 / optSet.NTrials;
            dynamic trial = optSet.Study.ask();
            var result = new EvaluatedGHResult();

            //TODO: Is this the correct way to handle the case of null?
            int nullCount = 0;
            while (nullCount < 10)
            {
                for (int j = 0; j < Variables.Count; j++)
                {
                    xTest[j] = Variables[j].IsInteger
                    ? trial.suggest_int(Variables[j].NickName, Variables[j].LowerBond, Variables[j].UpperBond, step: Variables[j].Epsilon)
                    : trial.suggest_float(Variables[j].NickName, Variables[j].LowerBond, Variables[j].UpperBond, step: Variables[j].Epsilon);
                }

                ProgressState pState = SetProgressState(optSet, xTest, trialNum, startTime);
                result = EvalFunc(pState, progress);
                optSet.HumanInTheLoop?.SaveNote(optSet.Study, trial, result.ActiveViewBitmap);

                if (result.ObjectiveValues.Contains(double.NaN))
                {
                    trial = optSet.Study.ask();
                    nullCount++;
                }
                else
                {
                    break;
                }
            }

            SetTrialUserAttr(result, trial, optSet);
            try
            {
                if (optSet.HumanInTheLoop == null)
                {
                    optSet.Study.tell(trial, result.ObjectiveValues.ToArray());
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

        private ProgressState SetProgressState(RunOptimizeSettings optSet, double[] xTest, int trialNum, DateTime startTime)
        {
            ComputeBestValues(optSet.Study, trialNum, out double[][] bestValues, out double hypervolumeRatio);
            return new ProgressState
            {
                TrialNumber = trialNum,
                ObjectiveNum = ObjNickName.Length,
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
                hypervolumeRatio = trialNum == 0 ? 0 : trialNum == 1 || ObjNickName.Length == 1 ? 1 : Hypervolume.Compute2dHypervolumeRatio(study);
            }
            else
            {
                bestValues = null;
                hypervolumeRatio = 0;
            }
        }

        private void CopyInMemoryStudy(dynamic storage)
        {
            dynamic optuna = Py.Import("optuna");
            string studyName = Settings.StudyName;
            optuna.copy_study(from_study_name: studyName, to_study_name: studyName, from_storage: storage, to_storage: new StorageHandler().CreateNewStorage(false, Settings.Storage));
        }

        private static dynamic EnqueueTrial(dynamic study, Dictionary<string, FishEgg> enqueueItems)
        {
            if (enqueueItems != null && enqueueItems.Count != 0)
            {
                for (int i = 0; i < enqueueItems.First().Value.Values.Count; i++)
                {
                    var enqueueDict = new PyDict();
                    foreach (KeyValuePair<string, FishEgg> enqueueItem in enqueueItems)
                    {
                        enqueueDict.SetItem(new PyString(enqueueItem.Key), new PyFloat(enqueueItem.Value.Values[i]));
                    }
                    study.enqueue_trial(enqueueDict, skip_if_exists: true);
                }
            }

            return study;
        }

        private void RunGC(EvaluatedGHResult result)
        {
            GcAfterTrial gcAfterTrial = Settings.Optimize.GcAfterTrial;
            if ((gcAfterTrial == GcAfterTrial.Always) ||
                (result.GeometryJson.Count > 0) &&
                (gcAfterTrial == GcAfterTrial.HasGeometry)
            )
            {
                dynamic gc = Py.Import("gc");
                gc.collect();
            }
        }

        private static void SetStudyUserAttr(dynamic study, StringBuilder variableName, StringBuilder objectiveName)
        {
            study.set_user_attr("variable_names", variableName.ToString());
            study.set_user_attr("objective_names", objectiveName.ToString());
            study.set_user_attr("tunny_version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        }

        private static void SetTrialUserAttr(EvaluatedGHResult result, dynamic trial, RunOptimizeSettings optSet)
        {
            if (result.GeometryJson.Count != 0)
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

            if (result.ObjectiveValues.Count != 0 && optSet.HumanInTheLoop != null)
            {
                for (int i = 0; i < result.ObjectiveValues.Count; i++)
                {
                    var key = new PyString("result_" + optSet.ObjectiveNames[i]);
                    var value = new PyFloat(result.ObjectiveValues[i]);
                    trial.set_user_attr(key, value);
                }
            }
        }

        private static void SetNonGeometricAttr(EvaluatedGHResult result, dynamic trial)
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
                case 6:
                    sampler = Sampler.TPE(optuna, Settings, hasConstraints);
                    break;
                case 1:
                    sampler = Sampler.BoTorch(optuna, Settings, hasConstraints);
                    break;
                case 2:
                    sampler = Sampler.NSGAII(optuna, Settings, hasConstraints);
                    break;
                case 3:
                    sampler = Sampler.CmaEs(optuna, Settings);
                    break;
                case 4:
                    sampler = Sampler.QMC(optuna, Settings);
                    break;
                case 5:
                    sampler = Sampler.Random(optuna, Settings);
                    break;
                default:
                    throw new ArgumentException("Unknown sampler type");
            }
            if (samplerType > 2 && hasConstraints)
            {
                TunnyMessageBox.Show("Only TPE, GP and NSGAII support constraints. Optimization is run without considering constraints.", "Tunny");
            }
            return sampler;
        }
    }

    public enum GcAfterTrial
    {
        Always,
        HasGeometry,
        NoExecute,
    }
}
