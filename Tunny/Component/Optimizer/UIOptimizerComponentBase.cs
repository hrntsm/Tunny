using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

using Tunny.Core.Util;
using Tunny.UI;
using Tunny.WPF;

namespace Tunny.Component.Optimizer
{
    public class UIOptimizeComponentBase : OptimizeComponentBase, IDisposable
    {
        internal OptimizationWindow OptimizationWindow;
        internal MainWindow MainWindow;

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
            TEnvVariables.GrasshopperWindowHandle = owner.Handle;

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

            var wpfThread = new Thread(new ThreadStart(() =>
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
                Dispatcher.Run();
            }));

            wpfThread.SetApartmentState(ApartmentState.STA);
            wpfThread.Start();
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

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas _, GH_CanvasMouseEvent e)
            {
                ((UIOptimizeComponentBase)Owner).MakeFishPrintByCaptureToTopOrder();
                ((UIOptimizeComponentBase)Owner).ShowOptimizationWindow();
                return GH_ObjectResponse.Handled;
            }
        }
    }
}
