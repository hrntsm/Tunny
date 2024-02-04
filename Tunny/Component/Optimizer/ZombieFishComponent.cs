using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.Kernel;

using Tunny.Component.Params;
using Tunny.Handler;
using Tunny.Settings;
namespace Tunny.Component.Optimizer
{
    public class ZombieFishComponent : OptimizeComponentBase
    {
        private int _count;
        private bool _running;
        private string _info;

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
            pManager.AddParameter(new Param_Fish(), "Fishes", "Fishes", "Fishes caught by the optimization nets.", GH_ParamAccess.list);
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
            DA.SetDataList(1, Fishes);
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
            GH_DocumentEditor ghCanvas = Instances.DocumentEditor;
            Message = "Finish";
            ghCanvas?.EnableUI();
        }

        public void SetInfo(string info)
        {
            _info = info;
        }

        protected override Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("c6b7612e-ee01-47ad-bc7d-713d76b274ab");
    }
}
