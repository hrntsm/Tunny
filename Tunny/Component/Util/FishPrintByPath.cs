using System;
using System.Drawing;

using Grasshopper.Kernel;

using Rhino;
using Rhino.Display;

using Tunny.Component.Params;
using Tunny.Resources;

namespace Tunny.Component.Util
{
    public class FishPrintByPath : GH_Component
    {
        public FishPrintByPath()
          : base("Fish Print by Path", "FPPath",
              "Create Fish Print by file path.",
              "Tunny", "Util")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "Path", "Create Fish Print by file path", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_FishPrint(), "FishPrint", "FPrint", "FishPrint", GH_ParamAccess.item);
        }

        private readonly RhinoDoc _doc = RhinoDoc.ActiveDoc;
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RhinoView activeView = _doc.Views.ActiveView;
            Bitmap bitmap = activeView.CaptureToBitmap();

            DA.SetData(0, bitmap);
        }

        protected override Bitmap Icon => Resource.FishPrintByPath;

        public override Guid ComponentGuid => new Guid("8ea46ce9-d546-4fdc-a950-976134af8227");
    }
}
