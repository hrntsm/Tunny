using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;

namespace Tunny.Type
{
    public class Param_FishEgg : GH_PersistentParam<GH_FishEgg>
    {
        public override GH_Exposure Exposure => GH_Exposure.primary;

        public Param_FishEgg()
          : base("Fish Egg", "Egg",
            "These eggs are enqueued for optimization and become fish.",
            "Tunny", "Tunny")
        {
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_FishEgg value) => GH_GetterResult.success;
        protected override GH_GetterResult Prompt_Plural(ref List<GH_FishEgg> values) => GH_GetterResult.success;
        protected override Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("62103679-967c-4255-921f-a1300d6a3b25");
    }
}
