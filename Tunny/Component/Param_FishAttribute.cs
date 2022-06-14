using System;

using Grasshopper.Kernel;

namespace Tunny.Type
{
    public class Param_FishAttribute : GH_Param<GH_FishAttribute>
    {
        public override GH_Exposure Exposure => GH_Exposure.hidden;

        public Param_FishAttribute(IGH_InstanceDescription tag)
         : base(tag)
        {
        }

        public Param_FishAttribute(IGH_InstanceDescription tag, GH_ParamAccess access)
         : base(tag, access)
        {
        }

        public Param_FishAttribute(string name, string nickname, string description, string category, string subcategory, GH_ParamAccess access)
         : base(name, nickname, description, category, subcategory, access)
        {
        }

        public override Guid ComponentGuid => new Guid("bfbd03eb-492d-4c57-8311-e7452d78a78e");
    }
}
