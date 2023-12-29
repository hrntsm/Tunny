using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using GalapagosComponents;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;

using Tunny.Component.Params;
using Tunny.Resources;
using Tunny.Type;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Component.Optimizer
{
    public partial class FishingComponent : GH_Component, IDisposable
    {
        internal OptimizationWindow OptimizationWindow;
        internal GrasshopperInOut GhInOut;
        internal Fish[] Fishes;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        public FishingComponent()
          : base("Tunny", "Tunny",
            "Tunny is an optimization component wrapped in optuna.",
            "Tunny", "Optimizer")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Variables", "Vars", "Connect variable number slider here.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Objectives", "Objs", "Connect objective number component here.", GH_ParamAccess.tree);
            pManager.AddParameter(new Param_FishAttribute(), "Attributes", "Attrs", "Connect model attribute like some geometry or values here. Not required.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Artifacts", "Artfs", "Connect artifacts here. Not required.", GH_ParamAccess.item);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_Fish(), "Fishes", "Fishes", "Fishes caught by the optimization nets.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CheckVariablesInput(Params.Input[0].Sources.Select(ghParam => ghParam.InstanceGuid));
            CheckObjectivesInput(Params.Input[1].Sources.Select(ghParam => ghParam.InstanceGuid));
            CheckArtifactsInput(Params.Input[3].Sources.Select(ghParam => ghParam.InstanceGuid));

            DA.SetDataList(0, Fishes);
        }

        private void CheckVariablesInput(IEnumerable<Guid> inputGuids)
        {
            foreach ((IGH_DocumentObject docObject, int _) in inputGuids.Select((guid, i) => (OnPingDocument().FindObject(guid, false), i)))
            {
                switch (docObject)
                {
                    case GH_NumberSlider slider:
                    case GalapagosGeneListObject genePool:
                    case Param_FishEgg fishEgg:
                        break;
                    default:
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"{docObject} input is not a valid variable.");
                        break;
                }
            }
        }

        private void CheckObjectivesInput(IEnumerable<Guid> inputGuids)
        {
            foreach ((IGH_DocumentObject docObject, int _) in inputGuids.Select((guid, i) => (OnPingDocument().FindObject(guid, false), i)))
            {
                switch (docObject)
                {
                    case Param_Number number:
                        break;
                    default:
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"{docObject} input is not a valid objective.");
                        break;
                }
            }
        }

        private void CheckArtifactsInput(IEnumerable<Guid> inputGuids)
        {
            foreach ((IGH_DocumentObject docObject, int _) in inputGuids.Select((guid, i) => (OnPingDocument().FindObject(guid, false), i)))
            {
                switch (docObject)
                {
                    case Param_Geometry geometry:
                    case Param_FishPrint fPrint:
                    case Param_String text:
                    case Param_FilePath filePath:
                        break;
                    default:
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"{docObject} input is not a valid artifact.");
                        break;
                }
            }
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
            m_attributes = new FishingComponentAttributes(this);
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
