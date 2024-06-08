using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Optuna.Study;

using Python.Runtime;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.Core.Storage;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Handler;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.Type;
using Tunny.UI;

namespace Tunny.Solver
{
    public class Algorithm : PythonInit
    {
        public Parameter[] OptimalParameters { get; private set; }
        public EndState EndState { get; private set; }

        private List<VariableBase> Variables { get; }
        private bool HasConstraints { get; }
        private Objective Objective { get; }
        private TSettings Settings { get; }
        private Func<ProgressState, int, TrialGrasshopperItems> EvalFunc { get; }
        private Dictionary<string, FishEgg> FishEgg { get; }

        public Algorithm(
            List<VariableBase> variables, bool hasConstraint, Objective objective, Dictionary<string, FishEgg> fishEgg,
            TSettings settings,
            Func<ProgressState, int, TrialGrasshopperItems> evalFunc)
        {
            TLog.MethodStart();
            Variables = variables;
            HasConstraints = hasConstraint;
            Objective = objective;
            FishEgg = fishEgg;
            Settings = settings;
            EvalFunc = evalFunc;
        }

        public void Solve()
        {
            TLog.MethodStart();
            EndState = EndState.Error;
            OptimizeLoop.IsForcedStopOptimize = false;
            SamplerType samplerType = Settings.Optimize.SelectSampler;
            int nTrials = Settings.Optimize.NumberOfTrials;
            double timeout = Settings.Optimize.Timeout <= 0 ? -1 : Settings.Optimize.Timeout;
            string[] directions = Objective.Directions;
            if (string.IsNullOrEmpty(Settings.StudyName))
            {
                Settings.StudyName = "no-name-" + Guid.NewGuid().ToString("D");
            }
            TLog.Info($"Optimization \"{Settings.StudyName}\" started with {nTrials} trials and {timeout} seconds timeout and {samplerType} sampler.");

            InitializePythonEngine();
            using (Py.GIL())
            {
                TLog.Debug("Wake Python GIL.");

                dynamic optuna = Py.Import("optuna");
                dynamic sampler = SetSamplerSettings(samplerType, HasConstraints, FishEgg);
                dynamic storage = Settings.Storage.CreateNewOptunaStorage(false);
                dynamic artifactBackend = Settings.Storage.CreateNewOptunaArtifactBackend(false);

                Parameter[] parameter = null;
                TrialGrasshopperItems result = null;

                if (CheckExistStudyMatching(Objective.Length))
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
            PythonEngine.Shutdown();
            TLog.Debug("Shutdown PythonEngine.");
        }

        private void PreferentialOptimization(int nBatch, dynamic storage, dynamic artifactBackend, out Parameter[] parameter, out TrialGrasshopperItems result, out dynamic study)
        {
            TLog.MethodStart();
            var preferentialOpt = new HumanInTheLoop.Preferential(TEnvVariables.TmpDirPath, Settings.Storage.Path);
            if (Objective.Length > 1)
            {
                TunnyMessageBox.Show("Human-in-the-Loop(Preferential GP optimization) only supports single objective optimization. Optimization is run without considering constraints.", "Tunny");
            }
            string[] objNickName = Objective.GetNickNames();
            study = preferentialOpt.CreateStudy(nBatch, Settings.StudyName, storage, objNickName[0]);
            var optInfo = new OptimizationHandlingInfo(int.MaxValue, 0, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, PyConverter.EnumeratorToPyList(Variables.Select(v => v.NickName)), false);
            HumanInTheLoop.HumanInTheLoopBase.WakeOptunaDashboard(Settings.Storage);
            optInfo.Preferential = preferentialOpt;
            RunPreferentialOptimize(optInfo, nBatch, out parameter, out result);
        }

        private void NormalOptimization(int nTrials, double timeout, string[] directions, dynamic sampler, dynamic storage, dynamic artifactBackend, out Parameter[] parameter, out TrialGrasshopperItems result, out dynamic study)
        {
            TLog.MethodStart();
            PyObject optuna = Py.Import("optuna");
            study = Study.CreateStudy(optuna, Settings.StudyName, sampler, directions, storage, Settings.Optimize.ContinueStudy);
            string[] objNickName = Objective.GetNickNames();
            var optInfo = new OptimizationHandlingInfo(nTrials, timeout, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, PyConverter.EnumeratorToPyList(Variables.Select(v => v.NickName)));
            RunOptimize(optInfo, out parameter, out result);
        }

        private void HumanSliderInputOptimization(int nBatch, double timeout, string[] directions, dynamic sampler, dynamic storage, dynamic artifactBackend, out Parameter[] parameter, out TrialGrasshopperItems result, out dynamic study)
        {
            TLog.MethodStart();
            PyObject optuna = Py.Import("optuna");
            study = Study.CreateStudy(optuna, Settings.StudyName, sampler, directions, storage, Settings.Optimize.ContinueStudy);
            string[] objNickName = Objective.GetNickNames();
            var optInfo = new OptimizationHandlingInfo(int.MaxValue, timeout, study, storage, artifactBackend, FishEgg, objNickName);
            SetStudyUserAttr(study, PyConverter.EnumeratorToPyList(Variables.Select(v => v.NickName)));
            var humanSliderInput = new HumanInTheLoop.HumanSliderInput(TEnvVariables.TmpDirPath, Settings.Storage.Path);
            HumanInTheLoop.HumanInTheLoopBase.WakeOptunaDashboard(Settings.Storage);
            humanSliderInput.SetObjective(study, objNickName);
            humanSliderInput.SetWidgets(study, objNickName);
            optInfo.HumanSliderInput = humanSliderInput;
            RunHumanSidlerInputOptimize(optInfo, nBatch, out parameter, out result);
        }

        private bool CheckExistStudyMatching(int nObjective)
        {
            TLog.MethodStart();
            var storage = new StorageHandler();
            StudySummary[] studySummaries = storage.GetStudySummaries(Settings.Storage.Path);
            bool containStudyName = studySummaries.Select(s => s.StudyName).Contains(Settings.StudyName);

            if (!containStudyName)
            {
                return true;
            }
            else if (!Settings.Optimize.ContinueStudy)
            {
                EndState = EndState.UseExitStudyWithoutContinue;
                return false;
            }

            bool isSameObjectiveNumber = studySummaries.FirstOrDefault(s => s.StudyName == Settings.StudyName)?.Directions.Length == nObjective;
            if (isSameObjectiveNumber)
            {
                return true;
            }
            else
            {
                EndState = EndState.DirectionNumNotMatch;
                return false;
            }
        }

        private void SetResultValues(int nObjective, dynamic study, Parameter[] parameter)
        {
            TLog.MethodStart();
            if (nObjective == 1 && Objective.HumanInTheLoopType != HumanInTheLoopType.Preferential)
            {
                PyObject[] pyBestParams = study.best_params.values();
                string[] values = pyBestParams.Select(x => x.ToString()).ToArray();
                string[] keys = (string[])study.best_params.keys();
                var opt = new Parameter[Variables.Count];

                for (int i = 0; i < Variables.Count; i++)
                {
                    for (int j = 0; j < keys.Length; j++)
                    {
                        if (keys[j] == Variables[i].NickName)
                        {
                            opt[i] = double.TryParse(values[j], out double num) ? new Parameter(num) : new Parameter(values[j]);
                        }
                    }
                }
                OptimalParameters = opt;
            }
            else
            {
                OptimalParameters = parameter;
            }
        }

        private void RunOptimize(OptimizationHandlingInfo optInfo, out Parameter[] parameter, out TrialGrasshopperItems result)
        {
            TLog.MethodStart();
            parameter = new Parameter[Variables.Count];
            result = new TrialGrasshopperItems();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optInfo.Study, optInfo.EnqueueItems);

            while (true)
            {
                if (result == null || CheckOptimizeComplete(optInfo, trialNum, startTime))
                {
                    break;
                }
                result = RunSingleOptimizeStep(optInfo, parameter, trialNum, startTime);
                trialNum++;
            }

            SaveInMemoryStudy(optInfo.Storage);
        }

        private void RunPreferentialOptimize(OptimizationHandlingInfo optInfo, int nBatch, out Parameter[] parameter, out TrialGrasshopperItems result)
        {
            TLog.MethodStart();
            parameter = new Parameter[Variables.Count];
            result = new TrialGrasshopperItems();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optInfo.Study, optInfo.EnqueueItems);

            while (true)
            {
                if (result == null || CheckOptimizeComplete(optInfo, trialNum, startTime))
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

        private void RunHumanSidlerInputOptimize(OptimizationHandlingInfo optInfo, int nBatch, out Parameter[] parameter, out TrialGrasshopperItems result)
        {
            TLog.MethodStart();
            parameter = new Parameter[Variables.Count];
            result = new TrialGrasshopperItems();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            EnqueueTrial(optInfo.Study, optInfo.EnqueueItems);

            while (true)
            {
                if (result == null || CheckOptimizeComplete(optInfo, trialNum, startTime))
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

        private TrialGrasshopperItems RunSingleOptimizeStep(OptimizationHandlingInfo optInfo, Parameter[] parameter, int trialNum, DateTime startTime)
        {
            TLog.MethodStart();
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
                SetOptimizationParameter(parameter, trial);
                ProgressState pState = SetProgressState(optInfo, parameter, trialNum, startTime);
                if (Settings.Optimize.IgnoreDuplicateSampling && IsSampleDuplicate(trial, out result))
                {
                    TLog.Info($"Trial {trialNum} is duplicate sample.");
                    pState.IsReportOnly = true;
                    EvalFunc(pState, progress);
                    break;
                }
                else
                {
                    result = EvalFunc(pState, progress);
                }

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
                    result.Artifacts.UploadArtifacts(optInfo.ArtifactBackend, trial);
                }

                if (result.Attribute.TryGetValue("IsFAIL", out List<string> isFail) && isFail.Contains("True"))
                {
                    dynamic optuna = Py.Import("optuna");
                    optInfo.Study.tell(trial, state: optuna.trial.TrialState.FAIL);
                    TLog.Warning($"Trial {trialNum} failed.");
                }
                else if (optInfo.HumanSliderInput == null && optInfo.Preferential == null)
                {
                    dynamic aa = optInfo.Study._stop_flag;
                    optInfo.Study.tell(trial, result.Objectives.Numbers);
                    SetTrialResultLog(trialNum, result, optInfo, parameter);
                }
            }
            catch (Exception e)
            {
                TLog.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
            finally
            {
                RunGC(result);
            }

            return result;
        }

        private static bool IsSampleDuplicate(dynamic trial, out TrialGrasshopperItems result)
        {
            double[] values;
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def check_duplicate(trial):\n" +
                "    import optuna\n" +
                "    from optuna.trial import TrialState\n" +
                "    states_to_consider = (TrialState.COMPLETE,)\n" +
                "    trials_to_consider = trial.study.get_trials(deepcopy=False, states=states_to_consider)\n" +
                "    for t in reversed(trials_to_consider):\n" +
                "        if trial.params == t.params:\n" +
                "            trial.set_user_attr('NOTE', f'trial {t.number} and trial {trial.number} were duplicate parameters.')\n" +
                "            if 'Constraint' in t.user_attrs:\n" +
                "               trial.set_user_attr('Constraint', t.user_attrs['Constraint'])\n" +
                "            return t.values\n" +
                "    return None\n"
            );
            dynamic checkDuplicate = ps.Get("check_duplicate");
            values = checkDuplicate(trial);
            result = new TrialGrasshopperItems(values);
            return values != null;
        }

        private void SetOptimizationParameter(Parameter[] parameter, dynamic trial)
        {
            TLog.MethodStart();
            foreach ((VariableBase variable, int i) in Variables.Select((v, i) => (v, i)))
            {
                string name = variable.NickName;
                switch (variable)
                {
                    case NumberVariable number:
                        double numParam = number.IsInteger
                            ? trial.suggest_int(name, number.LowerBond, number.UpperBond, step: number.Epsilon)
                            : trial.suggest_float(name, number.LowerBond, number.UpperBond, step: number.Epsilon);
                        parameter[i] = new Parameter(numParam);
                        break;
                    case CategoricalVariable category:
                        string catParam = (string)trial.suggest_categorical(name, category.Categories);
                        parameter[i] = new Parameter(catParam);
                        break;
                }
            }
        }

        private void SetTrialResultLog(int trialNum, TrialGrasshopperItems result, OptimizationHandlingInfo optInfo, Parameter[] parameter)
        {
            TLog.MethodStart();
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
            string message = sb.ToString();
            if (OptimizeLoop.Component is BoneFishComponent component)
            {
                component.SetInfo(message);
            }
            TLog.Info(sb.ToString());
        }

        private static TrialGrasshopperItems TenTimesNullResultErrorMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "The objective function returned NaN 10 times in a row. Tunny terminates the optimization. Please check the objective function.",
                "Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return null;
        }

        private bool CheckOptimizeComplete(OptimizationHandlingInfo optInfo, int trialNum, DateTime startTime)
        {
            TLog.MethodStart();

            int nTrials = optInfo.NTrials;
            double timeout = optInfo.Timeout;
            dynamic study = optInfo.Study;

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
            else if (study._stop_flag)
            {
                EndState = EndState.StoppedByOptuna;
                isOptimizeCompleted = true;
            }

            return isOptimizeCompleted;
        }

        private ProgressState SetProgressState(OptimizationHandlingInfo optSet, Parameter[] parameter, int trialNum, DateTime startTime)
        {
            TLog.MethodStart();
            double[][] bestValues = ComputeBestValues(optSet.Study);
            return new ProgressState
            {
                TrialNumber = trialNum,
                ObjectiveNum = Objective.Length,
                BestValues = bestValues,
                Parameter = parameter,
                HypervolumeRatio = 0,
                EstimatedTimeRemaining = optSet.Timeout <= 0
                    ? TimeSpan.FromSeconds((DateTime.Now - startTime).TotalSeconds * (optSet.NTrials - trialNum) / (trialNum + 1))
                    : TimeSpan.FromSeconds(optSet.Timeout - (DateTime.Now - startTime).TotalSeconds)
            };
        }

        private double[][] ComputeBestValues(dynamic study)
        {
            TLog.MethodStart();
            if (Settings.Optimize.ShowRealtimeResult)
            {
                dynamic[] bestTrials = study.best_trials;
                return bestTrials.Select(t => (double[])t.values).ToArray();
            }
            else
            {
                return null;
            }
        }

        private void SaveInMemoryStudy(dynamic storage)
        {
            TLog.MethodStart();
            if (Settings.Storage.Type == StorageType.InMemory)
            {
                dynamic optuna = Py.Import("optuna");
                string studyName = Settings.StudyName;
                optuna.copy_study(from_study_name: studyName, to_study_name: studyName, from_storage: storage, to_storage: new StorageHandler().CreateNewTStorage(false, Settings.Storage));
            }
        }

        private static dynamic EnqueueTrial(dynamic study, Dictionary<string, FishEgg> enqueueItems)
        {
            TLog.MethodStart();
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
            TLog.MethodStart();
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
            TLog.MethodStart();
            study.set_user_attr("variable_names", variableName);
            study.set_user_attr("tunny_version", TEnvVariables.Version.ToString(3));
            if (setMetricNames)
            {
                study.set_metric_names(PyConverter.EnumeratorToPyList(Objective.GetNickNames()));
            }
        }

        private static void SetTrialUserAttr(TrialGrasshopperItems result, dynamic trial, OptimizationHandlingInfo optSet)
        {
            TLog.MethodStart();
            if (result.GeometryJson.Length != 0)
            {
                PyList pyJson = PyConverter.EnumeratorToPyList(result.GeometryJson);
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
            TLog.MethodStart();
            foreach (KeyValuePair<string, List<string>> pair in result.Attribute)
            {
                PyList pyList;
                if (pair.Key == "Constraint")
                {
                    IEnumerable<double> values = pair.Value.Select(x => double.Parse(x, CultureInfo.InvariantCulture));
                    pyList = PyConverter.EnumeratorToPyList(values);
                }
                else if (pair.Key == "Direction")
                {
                    continue;
                }
                else
                {
                    pyList = PyConverter.EnumeratorToPyList(pair.Value);
                }
                trial.set_user_attr(pair.Key, pyList);
            }
        }

        private dynamic SetSamplerSettings(SamplerType samplerType, bool hasConstraints, Dictionary<string, FishEgg> fishEgg)
        {
            TLog.MethodStart();
            string storagePath = Settings.Storage.GetOptunaStoragePath();
            Dictionary<string, double> firstVariables = GetFirstEgg(fishEgg);

            dynamic optunaSampler = Settings.Optimize.Sampler.ToPython(samplerType, storagePath, hasConstraints, firstVariables);
            if (
                (samplerType == SamplerType.GP || samplerType == SamplerType.CmaEs || samplerType == SamplerType.QMC || samplerType == SamplerType.Random)
                && hasConstraints
            )
            {
                TunnyMessageBox.Show("Only TPE, GP:BoTorch and NSGA support constraints. Optimization is run without considering constraints.", "Tunny");
            }
            return optunaSampler;
        }

        private static Dictionary<string, double> GetFirstEgg(Dictionary<string, FishEgg> fishEgg)
        {
            TLog.MethodStart();
            if (fishEgg == null || fishEgg.Count == 0)
            {
                return null;
            }

            var firstVariables = new Dictionary<string, double>();
            foreach (KeyValuePair<string, FishEgg> pair in fishEgg)
            {
                firstVariables.Add(pair.Key, pair.Value.Values[0]);
            }

            return firstVariables;
        }
    }
}
