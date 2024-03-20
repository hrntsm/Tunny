using System;
using System.Drawing;

using Grasshopper.Kernel;

using Tunny.Component.Params;
using Tunny.Resources;

namespace Tunny.Component.Print
{
    public class FishPrintByPath : GH_Component
    {
        public FishPrintByPath()
          : base("Fish Print by Path", "FPPath",
              "Create Fish Print by file path.",
              "Tunny", "Print")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "Path", "Create Fish Print by file path", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_FishPrint(), "FishPrint", "FPrint", "FishPrint", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = string.Empty;
            DA.GetData(0, ref path);

            var bitmap = Image.FromFile(path) as Bitmap;
            DA.SetData(0, bitmap);
        }

        protected override Bitmap Icon => Resource.FishPrintByPath;
        public override Guid ComponentGuid => new Guid("8ea46ce9-d546-4fdc-a950-976134af8227");
    }
}
