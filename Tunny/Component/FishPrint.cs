﻿using System;
using System.Drawing;

using Grasshopper.Kernel;

using Rhino;
using Rhino.Display;
using Rhino.Geometry;

namespace MyNamespace
{
    public class FishPrint : GH_Component
    {
        public FishPrint()
          : base("FishPrint", "capture",
              "Description",
              "Tunny", "Subcategory")
        {
        }

        public override BoundingBox ClippingBox => new BoundingBox(new Point3d(-1e+10, -1e+10, -1e+10), new Point3d(1e+10, 1e+10, 1e+1));

        public override bool IsPreviewCapable => true;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Geometry", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FishPrint", "cap", "FishPrint", GH_ParamAccess.item);
        }

        private readonly RhinoDoc _doc = RhinoDoc.ActiveDoc;
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RhinoView activeView = _doc.Views.ActiveView;
            Bitmap bitmap = activeView.CaptureToBitmap();

            DA.SetData(0, bitmap);
        }

        protected override Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("5CFA19E3-2FBE-45D3-8E40-EF459C7FF491");
    }
}
