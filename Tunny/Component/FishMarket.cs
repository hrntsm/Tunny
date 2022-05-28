using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Grasshopper.Kernel;

using Rhino.Geometry;

using Tunny.GHType;

namespace Tunny.Component
{
    public class FishMarket : GH_Component
    {
        private readonly List<Point3d> _minPoints = new List<Point3d>();
        private List<GH_CFish> _cFishes = new List<GH_CFish>();

        public FishMarket()
          : base("FishMarket", "FMarket",
                 "A place to lay out the solutions we caught.",
                 "Params", "Optimize")
        {
        }

        public override void ClearData()
        {
            base.ClearData();
            _minPoints.Clear();
            _cFishes.Clear();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_CFish(), "CaughtFishes", "CaughtFishes", "Fishes caught by the optimization nets.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "Plane", "Plane", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddIntegerParameter("xNum", "xNum", "Number of x.", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("xInterval", "xInterval", "Interval of x.", GH_ParamAccess.item, 1000);
            pManager.AddNumberParameter("yInterval", "yInterval", "Interval of y.", GH_ParamAccess.item, 1000);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh of the fish market.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var cFishObjects = new List<object>();
            var origin = new Plane();
            int xNum = 0;
            double xInterval = 0;
            double yInterval = 0;
            if (!DA.GetDataList(0, cFishObjects)) { return; }
            if (!DA.GetData(1, ref origin)) { return; }
            if (!DA.GetData(2, ref xNum)) { return; }
            if (!DA.GetData(3, ref xInterval)) { return; }
            if (!DA.GetData(4, ref yInterval)) { return; }

            if (xNum <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "xNum must lager than 0");
                return;
            }

            int countY = 0;
            bool remainMesh = true;
            var translatedMeshes = new Mesh[cFishObjects.Count];
            _cFishes = cFishObjects.Select(x => (GH_CFish)x).ToList();
            var modelMeshes = _cFishes.Select(x => x.Value.ModelMesh).ToList();
            while (true)
            {
                Vector3d yVec = origin.YAxis * (yInterval * countY);
                for (int countX = 0; countX < xNum; countX++)
                {
                    int index = countX + xNum * countY;
                    if (index == translatedMeshes.Length)
                    {
                        remainMesh = false;
                        break;
                    }

                    Vector3d xVec = origin.XAxis * (xInterval * countX);
                    if (modelMeshes[index] != null)
                    {
                        Mesh mesh = modelMeshes[index];
                        Point3d modelMinPt = mesh.GetBoundingBox(Plane.WorldXY).Min;
                        mesh.Rotate(Vector3d.VectorAngle(Vector3d.XAxis, origin.XAxis), Vector3d.ZAxis, modelMinPt);
                        mesh.Translate(xVec + yVec + new Vector3d(origin.Origin) - new Vector3d(modelMinPt));
                        translatedMeshes[index] = mesh;
                        _minPoints.Add(modelMinPt);
                    }
                }
                if (!remainMesh)
                {
                    break;
                }
                countY++;
            }

            DA.SetDataList(0, translatedMeshes);
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            for (int i = 0; i < _cFishes.Count; i++)
            {
                args.Display.Draw2dText(_cFishes[i].ToString(), Color.Black, _minPoints[i], false, 10);
            }
        }

        protected override Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("46E05DFF-8EFD-418A-AC5B-7C9F24559B2E");
    }
}
