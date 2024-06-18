using System;
using System.Drawing;
using System.Windows.Forms;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

using Tunny.UI;

namespace Tunny.Component.Optimizer
{
    public class UIOptimizeComponentBase : OptimizeComponentBase, IDisposable
    {
        internal OptimizationWindow OptimizationWindow;

        public UIOptimizeComponentBase(string name, string nickname, string description)
          : base(name, nickname, description)
        {
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

        public override void CreateAttributes()
        {
            m_attributes = new UIOptimizerComponentAttributes(this);
        }

        private sealed class UIOptimizerComponentAttributes : OptimizerAttributeBase
        {
            public UIOptimizerComponentAttributes(IGH_Component component)
              : base(component, Color.CornflowerBlue, Color.DarkBlue, Color.Black)
            {
            }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((UIOptimizeComponentBase)Owner).MakeFishPrintByCaptureToTopOrder();
                ((UIOptimizeComponentBase)Owner).ShowOptimizationWindow();
                return GH_ObjectResponse.Handled;
            }
        }
    }
}
