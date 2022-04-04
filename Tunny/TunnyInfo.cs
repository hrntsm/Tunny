using System;
using System.Drawing;

using Grasshopper.Kernel;

namespace Tunny
{
    public class TunnyInfo : GH_AssemblyInfo
    {
        public override string Name => "Tunny Info";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "Tunny is an optimization component wrapped in optuna.";

        public override Guid Id => new Guid("01E58960-AFAA-48FF-BC90-174FDC4A9D64");

        //Return a string identifying you or your company.
        public override string AuthorName => "hrntsm";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "contact@hrntsm.com";
    }
}