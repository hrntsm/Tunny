using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Rhino.Geometry;

using Tunny.Component;
using Tunny.Settings;
using Tunny.Solver;
using Tunny.Type;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Handler
{
    internal static class OutputLoop
    {
        private static BackgroundWorker s_worker;
        private static TunnyComponent s_component;
        public static TunnySettings Settings;
        public static string StudyName;
        public static string[] NickNames;
        public static int[] Indices;
        public static OutputMode Mode;
        public static bool IsForcedStopOutput;

        internal static void Run(object sender, DoWorkEventArgs e)
        {
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as TunnyComponent;

            var fishes = new List<Fish>();

            var optunaSolver = new Optuna(s_component.GhInOut.ComponentFolder, Settings, s_component.GhInOut.HasConstraint);
            ModelResult[] modelResult = optunaSolver.GetModelResult(Indices, StudyName, s_worker);
            if (modelResult.Length == 0)
            {
                TunnyMessageBox.Show("There are no output models. Please check study name.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (ModelResult result in modelResult)
            {
                SetResultToFish(fishes, result, NickNames);
            }

            s_component.Fishes = fishes.ToArray();
            s_worker.ReportProgress(100);

            if (s_worker != null)
            {
                s_worker.Dispose();
            }
            TunnyMessageBox.Show("Output result to fish completed successfully.", "Tunny");
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
                if (objectives.ContainsKey(nickNames[i]))
                {
                    objectives.Add(nickNames[i] + i, model.Objectives[i]);
                }
                else
                {
                    objectives.Add(nickNames[i], model.Objectives[i]);
                }
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
                        if (!string.IsNullOrEmpty(json))
                        {
                            geometries.Add(Rhino.Runtime.CommonObject.FromJSON(json) as GeometryBase);
                        }
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
