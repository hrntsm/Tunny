using System;

using Grasshopper.Kernel;

using Tunny.Type;

namespace Tunny.Component
{
    public class DeconstructFishAttribute : GH_Component
    {
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public DeconstructFishAttribute()
          : base("Deconstruct Fish Attribute", "DeconFA",
              "Deconstruct Fish Attribute.",
              "Params", "Tunny")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_FishAttribute(), "Attributes", "Attrs", "Attributes to each Trial", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("D3B7B64A-71BA-41BE-9B2E-B0F459886B36");
    }
}
