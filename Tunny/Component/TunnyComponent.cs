using System;
using System.Drawing;
using System.Windows.Forms;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Tunny.Resources;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Component
{
    public partial class TunnyComponent : GH_Component
    {
        internal OptimizationWindow OptimizationWindow;
        internal GrasshopperInOut GhInOut;
        internal GH_Structure<GH_Mesh> ModelMesh = new GH_Structure<GH_Mesh>();
        internal GH_Structure<GH_Number> Objectives = new GH_Structure<GH_Number>();
        internal GH_Structure<GH_Number> Variables = new GH_Structure<GH_Number>();

        public override GH_Exposure Exposure => GH_Exposure.senary;

        public TunnyComponent()
          : base("Tunny", "Tunny",
              "Tunny is an optimization component wrapped in optuna.",
              "Params", "Util")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Variables", "Variables", "Connect variable number slider here.", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Objectives", "Objectives", "Connect objective number component here.", GH_ParamAccess.tree);
            pManager.AddMeshParameter("ModelMesh", "ModelMesh", "Connect model mesh here. Not required. Large size models are not recommended as it affects the speed of analysis.", GH_ParamAccess.tree);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("ResultVariables", "ResVar", "Result variables.", GH_ParamAccess.tree);
            pManager.AddNumberParameter("ResultObjectives", "ResObj", "Result objectives.", GH_ParamAccess.tree);
            pManager.AddMeshParameter("ResultMesh", "ResMesh", "Result model mesh.", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetDataTree(0, Variables);
            DA.SetDataTree(1, Objectives);
            DA.SetDataTree(2, ModelMesh);
        }

        public void GhInOutInstantiate()
        {
            GhInOut = new GrasshopperInOut(this);
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
