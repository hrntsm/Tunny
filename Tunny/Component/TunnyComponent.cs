using System;
using System.Drawing;
using System.Windows.Forms;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.Kernel;

using Tunny.Resources;
using Tunny.Type;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Component
{
    public partial class TunnyComponent : GH_Component, IDisposable
    {
        internal OptimizationWindow OptimizationWindow;
        internal GrasshopperInOut GhInOut;
        internal Fish[] Fishes;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        public TunnyComponent()
          : base("Tunny", "Tunny",
            "Tunny is an optimization component wrapped in optuna.",
            "Tunny", "Tunny")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Variables", "Vars", "Connect variable number slider here.", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Objectives", "Objs", "Connect objective number component here.", GH_ParamAccess.tree);
            pManager.AddParameter(new Param_FishAttribute(), "Attributes", "Attrs", "Connect model attribute like some geometry or values here. Not required.", GH_ParamAccess.item);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_Fish(), "Fishes", "Fishes", "Fishes caught by the optimization nets.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetDataList(0, Fishes);
        }

        public void GhInOutInstantiate()
        {
            GhInOut = new GrasshopperInOut(this);
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            base.RemovedFromDocument(document);
            if (OptimizationWindow != null)
            {
                OptimizationWindow.BGDispose();
                OptimizationWindow.Dispose();
            }
        }

        public void Dispose()
        {
            if (OptimizationWindow != null)
            {
                OptimizationWindow.BGDispose();
                OptimizationWindow.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public override void CreateAttributes()
        {
            m_attributes = new TunnyAttributes(this);
        }

        private void ShowOptimizationWindow()
        {
            GH_DocumentEditor owner = Instances.DocumentEditor;

            if (OptimizationWindow == null || OptimizationWindow.IsDisposed)
            {
                OptimizationWindow = new OptimizationWindow(this)
                {
                    StartPosition = FormStartPosition.Manual
                };

                GH_WindowsFormUtil.CenterFormOnWindow(OptimizationWindow, owner, true);
                owner.FormShepard.RegisterForm(OptimizationWindow);
            }
            OptimizationWindow.Show(owner);
        }

        protected override Bitmap Icon => Resource.TunnyIcon;
        public override Guid ComponentGuid => new Guid("701d2c47-1440-4d09-951c-386200e29b28");
    }
}
