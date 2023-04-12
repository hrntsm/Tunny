using System;
using System.Drawing;

using Grasshopper.Kernel;

using Tunny.Resources;

namespace Tunny
{
    public class Tunny : GH_AssemblyInfo
    {
        public override string Name => "Tunny";
        public override string Version => "0.7.2";
        public override Bitmap Icon => Resource.TunnyIcon;
        public override string Description => "Tunny is an optimization component wrapped in optuna.";
        public override Guid Id => new Guid("01E58960-AFAA-48FF-BC90-174FDC4A9D64");
        public override string AuthorName => "hrntsm";
        public override string AuthorContact => "contact@hrntsm.com";
        public override GH_LibraryLicense License => GH_LibraryLicense.opensource;
    }

    public class TunnyCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("Tunny", Resource.TunnyIcon);
            Grasshopper.Instances.ComponentServer.AddCategorySymbolName("Tunny", 'T');
            return GH_LoadingInstruction.Proceed;
        }
    }
}
