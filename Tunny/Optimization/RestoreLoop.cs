using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.FileIO;
using Rhino.Geometry;

using Tunny.Component;
using Tunny.Solver;
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

        internal static void Run(object sender, DoWorkEventArgs e)
        {
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as TunnyComponent;

            var modelMesh = new GH_Structure<GH_Mesh>();
            var variables = new GH_Structure<GH_Number>();
            var objectives = new GH_Structure<GH_Number>();

            var optunaSolver = new Optuna(s_component.GhInOut.ComponentFolder);
            ModelResult[] modelResult = optunaSolver.GetModelResult(Indices, StudyName);
            if (modelResult.Length == 0)
            {
                TunnyMessageBox.Show("There are no restore models. Please check study name.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < modelResult.Length; i++)
            {
                SetVariables(variables, modelResult[i], NickNames);
                SetObjectives(objectives, modelResult[i]);
                SetModelMesh(modelMesh, modelResult[i]);
                s_worker.ReportProgress(i * 100 / modelResult.Length);
            }
            s_component.Variables = variables;
            s_component.Objectives = objectives;
            s_component.ModelMesh = modelMesh;
            s_worker.ReportProgress(100);

            if (s_worker != null)
            {
                s_worker.CancelAsync();
            }
            TunnyMessageBox.Show("Restore completed successfully.", "Tunny");
        }

        private static void SetVariables(GH_Structure<GH_Number> objectives, ModelResult model, IEnumerable<string> nickName)
        {
            foreach (string name in nickName)
            {
                foreach (KeyValuePair<string, double> obj in model.Variables.Where(obj => obj.Key == name))
                {
                    objectives.Append(new GH_Number(obj.Value), new GH_Path(0, model.Number));
                }
            }
        }

        private static void SetObjectives(GH_Structure<GH_Number> objectives, ModelResult model)
        {
            foreach (double obj in model.Objectives)
            {
                objectives.Append(new GH_Number(obj), new GH_Path(0, model.Number));
            }
        }

        private static void SetModelMesh(GH_Structure<GH_Mesh> modelMesh, ModelResult model)
        {
            if (model.Draco == string.Empty)
            {
                return;
            }
            var mesh = (Mesh)DracoCompression.DecompressBase64String(model.Draco);
            modelMesh.Append(new GH_Mesh(mesh), new GH_Path(0, model.Number));
        }
    }
}