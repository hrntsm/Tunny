using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Optuna.Trial;

using Rhino.Geometry;
using Rhino.Runtime;

using Tunny.Component.Optimizer;
using Tunny.Core.Settings;
using Tunny.Core.Solver;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Type;
using Tunny.UI;

namespace Tunny.Handler
{
    internal static class OutputLoop
    {
        private static BackgroundWorker s_worker;
        public static OptimizeComponentBase Component;
        public static TSettings Settings;
        public static string StudyName;
        public static string[] NickNames;
        public static int[] Indices;
        public static OutputMode Mode;
        public static bool IsForcedStopOutput;

        internal static void Run(object sender, DoWorkEventArgs e)
        {
            TLog.MethodStart();
            s_worker = sender as BackgroundWorker;
            Component = e.Argument as UIOptimizeComponentBase;

            var fishes = new List<Fish>();

            if (Component != null)
            {
                var output = new Output(Settings.Storage.Path);
                Trial[] targetTrials = output.GetTargetTrial(Indices, StudyName);
                string[] metricNames = output.GetMetricNames(StudyName);
                if (targetTrials.Length == 0)
                {
                    TunnyMessageBox.Show("There are no output models. Please check study name.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Component.GhInOut.HasConstraint && Indices[0] == -1)
                {
                    TunnyMessageBox.Show("Pareto solution is output with no constraints taken into account.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (metricNames == null || metricNames.Length == 0)
                {
                    metricNames = Component.GhInOut.Objectives.GetNickNames();
                    if (metricNames.Length != targetTrials[0].Values.Length)
                    {
                        TunnyMessageBox.Show("The number of objective functions in the result file does not match the number of objective functions in the Grasshopper file.\nThe two should be the same number.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                foreach (Trial trial in targetTrials)
                {
                    SetResultToFish(fishes, trial, NickNames, metricNames);
                }
                Component.Fishes = fishes.ToArray();
            }

            s_worker?.ReportProgress(100);
            s_worker?.Dispose();
            TunnyMessageBox.Show("Output result to fish completed successfully.", "Tunny");
        }

        public static void SetResultToFish(List<Fish> fishes, Trial trial, IEnumerable<string> varNickname, string[] objNickname)
        {
            TLog.MethodStart();
            fishes.Add(new Fish
            {
                ModelNumber = trial.Number,
                Variables = SetVariables(trial.Params, varNickname),
                Objectives = SetObjectives(trial.Values, objNickname),
                Attributes = SetAttributes(ParseAttrs(trial.UserAttrs)),
            });
        }

        private static Dictionary<string, object> SetVariables(Dictionary<string, object> variables, IEnumerable<string> nickNames)
        {
            TLog.MethodStart();
            return nickNames.SelectMany(name => variables.Where(obj => obj.Key == name))
                .ToDictionary(variable => variable.Key, variable => variable.Value);
        }

        private static Dictionary<string, double> SetObjectives(double[] values, string[] nickNames)
        {
            TLog.MethodStart();
            var objectives = new Dictionary<string, double>();
            if (values == null)
            {
                return null;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (objectives.ContainsKey(nickNames[i]))
                {
                    objectives.Add(nickNames[i] + i, values[i]);
                }
                else
                {
                    objectives.Add(nickNames[i], values[i]);
                }
            }
            return objectives;
        }

        private static Dictionary<string, object> SetAttributes(Dictionary<string, List<string>> trialAttr)
        {
            TLog.MethodStart();
            var attribute = new Dictionary<string, object>();
            foreach (KeyValuePair<string, List<string>> attr in trialAttr)
            {
                if (attr.Key == "Geometry")
                {
                    var geometries = attr.Value.Where(json => !string.IsNullOrEmpty(json))
                        .Select(json => CommonObject.FromJSON(json) as GeometryBase)
                        .ToList();
                    attribute.Add(attr.Key, geometries);
                }
                else
                {
                    attribute.Add(attr.Key, attr.Value);
                }
            }
            return attribute;
        }

        private static Dictionary<string, List<string>> ParseAttrs(Dictionary<string, object> userAttrs)
        {
            TLog.MethodStart();
            var attributes = new Dictionary<string, List<string>>();
            foreach (string key in userAttrs.Keys)
            {
                string[] values = userAttrs[key] as string[];
                attributes.Add(key, values.ToList());
            }
            return attributes;
        }
    }
}
