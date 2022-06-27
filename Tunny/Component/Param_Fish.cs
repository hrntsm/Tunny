using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;

using Tunny.Resources;
using Tunny.Type;

namespace Tunny.Component
{
    public class Param_Fish : GH_PersistentParam<GH_Fish>
    {
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public Param_Fish()
          : base("Fish", "Fish",
            "Fish caught by the optimization nets",
            "Tunny", "Tunny")
        {
        }

        protected override GH_Fish InstantiateT() => new GH_Fish();
        protected override GH_Fish PreferredCast(object data) => data is Fish fish ? new GH_Fish(fish) : (GH_Fish)null;
        protected override GH_GetterResult Prompt_Singular(ref GH_Fish value) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Plural(ref List<GH_Fish> values) => GH_GetterResult.success;

        protected override Bitmap Icon => Resource.ParamFishIcon;
        public override Guid ComponentGuid => new Guid("172b2271-1465-49f9-91f6-64d6eeef4487");
    }
}
