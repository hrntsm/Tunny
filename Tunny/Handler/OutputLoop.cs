using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Rhino.Geometry;
using Rhino.Runtime;

using Tunny.Component.Optimizer;
using Tunny.Enum;
using Tunny.PostProcess;
using Tunny.Settings;
using Tunny.Type;
using Tunny.UI;

namespace Tunny.Handler
{
    internal static class OutputLoop
    {
        private static BackgroundWorker s_worker;
        private static FishingComponent s_component;
        public static TunnySettings Settings;
        public static string StudyName;
        public static string[] NickNames;
        public static int[] Indices;
        public static OutputMode Mode;
        public static bool IsForcedStopOutput;

        internal static void Run(object sender, DoWorkEventArgs e)
        {
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as FishingComponent;

            var fishes = new List<Fish>();

            if (s_component != null)
            {
                var optunaSolver = new Solver.Optuna(s_component.GhInOut.ComponentFolder, Settings, s_component.GhInOut.HasConstraint);
                ModelResult[] modelResult = optunaSolver.GetModelResult(Indices, StudyName, s_worker);
                if (modelResult.Length == 0)
                {
                    TunnyMessageBox.Show("There are no output models. Please check study name.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (s_component.GhInOut.HasConstraint)
                {
                    TunnyMessageBox.Show("Pareto solution is output with no constraints taken into account.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                foreach (ModelResult result in modelResult)
                {
                    SetResultToFish(fishes, result, NickNames);
                }
                s_component.Fishes = fishes.ToArray();
            }

            s_worker?.ReportProgress(100);
            s_worker?.Dispose();
            TunnyMessageBox.Show("Output result to fish completed successfully.", "Tunny");
        }

        private static void SetResultToFish(List<Fish> fishes, ModelResult model, IEnumerable<string> nickname)
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
            return nickNames.SelectMany(name => model.Variables.Where(obj => obj.Key == name))
                .ToDictionary(variable => variable.Key, variable => variable.Value);
        }

        private static Dictionary<string, double> SetObjectives(ModelResult model)
        {
            string[] nickNames = s_component.GhInOut.Objectives.GetNickNames();
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
    }
}
