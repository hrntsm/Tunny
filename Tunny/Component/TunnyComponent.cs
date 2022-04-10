using System;
using System.Drawing;
using System.Windows.Forms;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

using Tunny.Resources;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Component
{
    public class TunnyComponent : GH_Component
    {
        internal OptimizationWindow OptimizationWindow;
        internal GrasshopperInOut GhInOut;

        public override GH_Exposure Exposure => GH_Exposure.senary;

        public TunnyComponent()
          : base("Tunny", "Tunny",
              "Tunny is an optimization component wrapped in optuna.",
              "Params", "Util")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Variables", "Variables", "Connect variable number slider here.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Objective", "Objective", "Connect objective number component here", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public void GhInOutInstantiate()
        {
            GhInOut = new GrasshopperInOut(this);
        }

        public override void CreateAttributes()
        {
            m_attributes = new AttributesA(this);
        }

        private class AttributesA : GH_ComponentAttributes
        {
            public AttributesA(IGH_Component component) : base(component)
            {
            }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((TunnyComponent)Owner).ShowOptimizationWindow();
                return GH_ObjectResponse.Handled;
            }
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