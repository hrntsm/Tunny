using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;

using Tunny.GHType;

namespace Tunny.Component
{
    public class Param_CFish : GH_PersistentParam<GH_CFish>
    {
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public Param_CFish()
         : base("CFish", "CFish",
                "Fish caught by the optimization nets",
                "Params", "Optimize")
        {
        }

        protected override GH_CFish InstantiateT() => new GH_CFish();
        protected override GH_CFish PreferredCast(object data) => data is CFish cFish ? new GH_CFish(cFish) : (GH_CFish)null;
        protected override GH_GetterResult Prompt_Singular(ref GH_CFish value) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Plural(ref List<GH_CFish> values) => GH_GetterResult.success;

        protected override Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("172b2271-1465-49f9-91f6-64d6eeef4487");
    }
}
