using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GalapagosComponents;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;

using Tunny.Component.Params;
using Tunny.Component.Util;
using Tunny.Core.Input;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Type;
using Tunny.Util;

namespace Tunny.Component.Optimizer
{
    public class OptimizeComponentBase : GH_Component
    {
        internal GrasshopperInOut GhInOut;
        internal GrasshopperStates GrasshopperStatus;
        internal Fish[] Fishes;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        public OptimizeComponentBase(string name, string nickname, string description)
          : base(name, nickname, description, "Tunny", "Optimizer")
        {
        }

        /// <summary>
        /// Change calculation order to FishPrintByCapture after all components have been calculated.
        /// </summary>
        protected void MakeFishPrintByCaptureToTopOrder()
        {
            TLog.MethodStart();
            IList<IGH_DocumentObject> objs = OnPingDocument().Objects;
            var fishPrints = new List<FishPrintByCapture>();
            foreach (IGH_DocumentObject obj in objs)
            {
                if (obj is FishPrintByCapture fp)
                {
                    fishPrints.Add(fp);
                }
            }

            if (fishPrints.Count > 0)
            {
                OnPingDocument().ArrangeObjects(fishPrints, GH_Arrange.MoveToFront);
            }
        }

        public void GhInOutInstantiate()
        {
            TLog.Info("Instantiate GrasshopperInOut");
            GhInOut = new GrasshopperInOut(this);
        }

        public void UpdateGrasshopper(IList<Parameter> parameters)
        {
            GrasshopperStatus = GrasshopperStates.RequestProcessing;
            GhInOut.NewSolution(parameters);
            GrasshopperStatus = GrasshopperStates.RequestProcessed;
        }

        public override void CreateAttributes()
        {
            m_attributes = new OptimizerAttributeBase(this, Color.DimGray, Color.Black, Color.White);
        }

        protected void CheckVariablesInput(IEnumerable<Guid> inputGuids)
        {
            foreach ((IGH_DocumentObject docObject, int _) in inputGuids.Select((guid, i) => (OnPingDocument().FindObject(guid, false), i)))
            {
                switch (docObject)
                {
                    case GH_ValueList _:
                    case GH_NumberSlider _:
                    case GalapagosGeneListObject _:
                    case Param_FishEgg _:
                        break;
                    default:
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"{docObject} input is not a valid variable.");
                        break;
                }
            }
        }

        protected void CheckObjectivesInput(IEnumerable<Guid> inputGuids)
        {
            foreach ((IGH_DocumentObject docObject, int _) in inputGuids.Select((guid, i) => (OnPingDocument().FindObject(guid, false), i)))
            {
                switch (docObject)
                {
                    case Param_Number number:
                    case Param_FishPrint fPrint:
                        break;
                    default:
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"{docObject} input is not a valid objective.");
                        break;
                }
            }
        }

        protected void CheckArtifactsInput(IEnumerable<Guid> inputGuids)
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

        public override Guid ComponentGuid => throw new NotImplementedException();

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            throw new NotImplementedException();
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            throw new NotImplementedException();
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            throw new NotImplementedException();
        }
    }
}
