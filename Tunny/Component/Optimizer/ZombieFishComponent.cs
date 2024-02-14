using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;

using Tunny.Component.Params;
using Tunny.Handler;
using Tunny.PostProcess;
using Tunny.Settings;
using Tunny.Type;
namespace Tunny.Component.Optimizer
{
    public class ZombieFishComponent : OptimizeComponentBase
    {
        private int _count;
        private bool _running;
        private string _info;
        private Fish[] _allFishes;

        public ZombieFishComponent()
          : base("Zombie Fish", "Zombie",
            "Zombie Fish is an optimization component wrapped in optuna without UI."
            )
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Variables", "Vars", "Connect variable number slider here.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Objectives", "Objs", "Connect objective number component here.", GH_ParamAccess.tree);
            pManager.AddParameter(new Param_FishAttribute(), "Attributes", "Attrs", "Connect model attribute like some geometry or values here. Not required.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Artifacts", "Artfs", "Connect artifacts here. Not required.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Start", "Start", "Start optimization.", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Stop", "Stop", "Stop optimization.", GH_ParamAccess.item, false);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
            Params.Input[5].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Info", "Info", "Information about the optimization.", GH_ParamAccess.item);
            pManager.AddParameter(new Param_Fish(), "All Fishes", "All", "All optimization results", GH_ParamAccess.list);
            pManager.AddParameter(new Param_Fish(), "BestFishes", "Best", "Best optimization results", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CheckVariablesInput(Params.Input[0].Sources.Select(ghParam => ghParam.InstanceGuid));
            CheckObjectivesInput(Params.Input[1].Sources.Select(ghParam => ghParam.InstanceGuid));
            CheckArtifactsInput(Params.Input[3].Sources.Select(ghParam => ghParam.InstanceGuid));

            bool start = false;
            bool stop = false;
            if (!DA.GetData(4, ref start)) { return; }
            if (!DA.GetData(5, ref stop)) { return; }

            if (!start)
            {
                Message = "No Opt";
                _info = "No optimization has been performed yet.";
            }

            if (start && stop)
            {
                OptimizeLoop.IsForcedStopOptimize = true;
            }

            if (start && !_running)
            {
                _running = true;
                _count = 0;
                GH_DocumentEditor ghCanvas = Instances.DocumentEditor;
                ghCanvas?.DisableUI();

                OptimizeLoop.Settings = TunnySettings.LoadFromJson();
                var worker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true,
                };
                worker.DoWork += OptimizeLoop.RunMultiple;
                worker.ProgressChanged += OptimizeProgressChangedHandler;
                worker.RunWorkerCompleted += StopOptimize;

                worker.RunWorkerAsync(this);
            }

            DA.SetData(0, _info);
            DA.SetDataList(1, _allFishes);
            DA.SetDataList(2, Fishes);
        }

        private void OptimizeProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            _count++;
            var pState = (ProgressState)e.UserState;
            UpdateGrasshopper(pState.Parameter);
            Message = $"Trial {_count}";
        }

        private void StopOptimize(object sender, RunWorkerCompletedEventArgs e)
        {
            _running = false;
            OptimizeLoop.IsForcedStopOptimize = true;

            Message = "Outputting";
            OutputLoop.Component = this;
            var optunaSolver = new Solver.Optuna(OptimizeLoop.Settings, GhInOut.HasConstraint);
            List<Fish> allFishes = GetFishes(optunaSolver, -10);
            _allFishes = allFishes.ToArray();
            Params.Output[1].ClearData();
            Params.Output[1].AddVolatileDataList(new GH_Path(0), allFishes.Select(x => new GH_Fish(x)));

            List<Fish> bestFishes = GetFishes(optunaSolver, -1);
            Fishes = bestFishes.ToArray();
            Params.Output[2].ClearData();
            Params.Output[2].AddVolatileDataList(new GH_Path(0), bestFishes.Select(x => new GH_Fish(x)));

            GH_DocumentEditor ghCanvas = Instances.DocumentEditor;
            Message = "Finish";
            ghCanvas?.EnableUI();
        }

        private List<Fish> GetFishes(Solver.Optuna optunaSolver, int outputMode)
        {
            ModelResult[] results = optunaSolver.GetModelResult(new[] { outputMode }, OptimizeLoop.Settings.StudyName, null);
            var fishes = new List<Fish>();
            foreach (ModelResult result in results)
            {
                OutputLoop.SetResultToFish(fishes, result, GhInOut.Variables.Select(x => x.NickName).ToArray());
            }

            return fishes;
        }

        public void SetInfo(string info)
        {
            _info = info;
        }

        protected override Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("c6b7612e-ee01-47ad-bc7d-713d76b274ab");
    }
}
