using System;
using System.Windows.Forms;

using BayesOpt.UI;
using BayesOpt.Util;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace BayesOpt.Component
{
    public class WithUI : GH_Component
    {
        internal OptimizationWindow OptimizationWindow;
        internal GrasshopperInOut GhInOut;

        public WithUI()
          : base("component", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Variables", "Variables", "The value of the optimization problem's objective function depends on variables, the unknowns. Connect an arbitrary number of number sliders and gene pools as the variables of the optimization problem.\n\nThe number sliders' bounds are the bounds of the variables. After optimization, the number sliders (variables) are set to the optimal position.\n\nIn Galapagos, the variables are called genes.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Objective", "Objective", "The objective value depends on the values of the variables. The algorithm searches for the objective's optimal value by changing the variables' values.\n\nThe objective value should be a single Number Parameter that yields a single value for any configuration of the variables.\n\nIn Galapagos, the objective value is called fitness.", GH_ParamAccess.item);
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
                ((WithUI)Owner).ShowOptimizationWindow();
                return GH_ObjectResponse.Handled;
            }
        }

        private void ShowOptimizationWindow()
        {
            var owner = Grasshopper.Instances.DocumentEditor;

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

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("701d2c47-1440-4d09-951c-386200e29b28");
    }
}