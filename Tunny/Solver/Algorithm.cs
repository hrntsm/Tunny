using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Python.Runtime;

using Tunny.Settings;
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
        private Func<double[], int, EvaluatedGHResult> EvalFunc { get; set; }
        private double[] XOpt { get; set; }
        private double[] FxOpt { get; set; }
        public EndState EndState { get; set; }

        public Algorithm(
            List<Variable> variables, bool hasConstraint, string[] objNickName,
            TunnySettings settings,
            Func<double[], int, EvaluatedGHResult> evalFunc)
        {
            Variables = variables;
            HasConstraints = hasConstraint;
            ObjNickName = objNickName;
            Settings = settings;
            EvalFunc = evalFunc;
        }

        public void Solve()
        {
            EndState = EndState.Error;
            Handler.Optimize.IsForcedStopOptimize = false;
            int samplerType = Settings.Optimize.SelectSampler;
            int nTrials = Settings.Optimize.NumberOfTrials;
            double timeout = Settings.Optimize.Timeout <= 0 ? double.MaxValue : Settings.Optimize.Timeout;
            int nObjective = ObjNickName.Length;
            string[] directions = SetDirectionValues(nObjective);

            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic sampler = SetSamplerSettings(samplerType, ref nTrials, optuna, HasConstraints);

                if (CheckExistStudyParameter(nObjective, optuna))
                {
                    dynamic study = CreateStudy(directions, optuna, sampler);
                    SetStudyUserAttr(study, ObjectNicknameToAttr());
                    RunOptimize(nTrials, timeout, study, out double[] xTest, out EvaluatedGHResult result);
                    SetResultValues(nObjective, study, xTest, result);
                }
            }
            PythonEngine.Shutdown();
        }

        private StringBuilder ObjectNicknameToAttr()
        {
            var name = new StringBuilder();
            foreach (string objName in ObjNickName)
            {
                name.Append(objName + ",");
            }
            name.Remove(name.Length - 1, 1);
            return name;
        }

        private dynamic CreateStudy(string[] directions, dynamic optuna, dynamic sampler)
        {
            return optuna.create_study(
                sampler: sampler,
                directions: directions,
                storage: "sqlite:///" + Settings.Storage,
                study_name: Settings.StudyName,
                load_if_exists: Settings.Optimize.LoadExistStudy
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

        private bool CheckExistStudyParameter(int nObjective, dynamic optuna)
        {
            PyList studySummaries = optuna.get_all_study_summaries("sqlite:///" + Settings.Storage);
            var studySummaryDict = new Dictionary<string, int>();

            foreach (dynamic pyObj in studySummaries)
            {
                studySummaryDict.Add((string)pyObj.study_name, (int)pyObj.directions.__len__());
            }

            return !studySummaryDict.ContainsKey(Settings.StudyName) || CheckDirections(nObjective, studySummaryDict);
        }

        private bool CheckDirections(int nObjective, Dictionary<string, int> directions)
        {
            if (!Settings.Optimize.LoadExistStudy)
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

        private void RunOptimize(int nTrials, double timeout, dynamic study, out double[] xTest, out EvaluatedGHResult result)
        {
            xTest = new double[Variables.Count];
            result = new EvaluatedGHResult();
            int trialNum = 0;
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (trialNum >= nTrials)
                {
                    EndState = EndState.AllTrialCompleted;
                    break;
                }
                else if ((DateTime.Now - startTime).TotalSeconds >= timeout)
                {
                    EndState = EndState.Timeout;
                    break;
                }
                else if (Handler.Optimize.IsForcedStopOptimize)
                {
                    EndState = EndState.StoppedByUser;
                    Handler.Optimize.IsForcedStopOptimize = false;
                    break;
                }

                int progress = trialNum * 100 / nTrials;
                dynamic trial = study.ask();

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
                    result = EvalFunc(xTest, progress);

                    if (result.ObjectiveValues.Contains(double.NaN))
                    {
                        trial = study.ask();
                        nullCount++;
                    }
                    else
                    {
                        break;
                    }
                }

                SetTrialUserAttr(result, trial);
                try
                {
                    study.tell(trial, result.ObjectiveValues.ToArray());
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message);
                }
                finally
                {
                    RunGC(result);
                }
                trialNum++;
            }
        }

        private void RunGC(EvaluatedGHResult result)
        {
            GcAfterTrial gcAfterTrial = Settings.Optimize.GcAfterTrial;
            if (gcAfterTrial == GcAfterTrial.Always ||
                result.GeometryJson.Count > 0 && gcAfterTrial == GcAfterTrial.HasGeometry
            )
            {
                dynamic gc = Py.Import("gc");
                gc.collect();
            }
        }

        private static void SetStudyUserAttr(dynamic study, StringBuilder name)
        {
            study.set_user_attr("objective_names", name.ToString());
            study.set_user_attr("tunny_version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        }

        private static void SetTrialUserAttr(EvaluatedGHResult result, dynamic trial)
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
                        pyList.Append(new PyFloat(double.Parse(str)));
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

        private dynamic SetSamplerSettings(int samplerType, ref int nTrials, dynamic optuna, bool hasConstraints)
        {
            dynamic sampler;
            switch (samplerType)
            {
                case 0:
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
                case 6:
                    sampler = Sampler.Grid(optuna, Variables, ref nTrials);
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

        public double[] GetXOptimum()
        {
            return XOpt;
        }

        public double[] GetFxOptimum()
        {
            return FxOpt;
        }

    }

    public enum GcAfterTrial
    {
        Always,
        HasGeometry,
        NoExecute,
    }
}
