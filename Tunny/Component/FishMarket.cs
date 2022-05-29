using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Display;
using Rhino.Geometry;

using Tunny.GHType;

namespace Tunny.Component
{
    public class FishMarket : GH_Component
    {
        private readonly List<Plane> _tagPlanes = new List<Plane>();
        private List<GH_CFish> _cFishes = new List<GH_CFish>();
        private double _size = 1;

        public FishMarket()
          : base("FishMarket", "FMarket",
                 "A place to lay out the solutions we caught.",
                 "Params", "Optimize")
        {
        }

        public override void ClearData()
        {
            base.ClearData();
            _tagPlanes.Clear();
            _cFishes.Clear();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Fish(), "Fishes", "Fishes", "Fishes caught by the optimization nets.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "Plane", "Plane", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddIntegerParameter("xNum", "xNum", "Number of x.", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("xInterval", "xInterval", "Interval of x.", GH_ParamAccess.item, 1000);
            pManager.AddNumberParameter("yInterval", "yInterval", "Interval of y.", GH_ParamAccess.item, 1000);
            pManager.AddNumberParameter("TextSize", "tSize", "Font size.", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh of the fish market.", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var cFishObjects = new List<object>();
            var plane = new Plane();
            int xNum = 0;
            double xInterval = 0;
            double yInterval = 0;
            if (!DA.GetDataList(0, cFishObjects)) { return; }
            if (!DA.GetData(1, ref plane)) { return; }
            if (!DA.GetData(2, ref xNum)) { return; }
            if (!DA.GetData(3, ref xInterval)) { return; }
            if (!DA.GetData(4, ref yInterval)) { return; }
            if (!DA.GetData(5, ref _size)) { return; }


            if (xNum <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "xNum must lager than 0");
                return;
            }

            int countY = 0;
            bool remainMesh = true;
            var translatedMeshes = new GH_Structure<GH_Mesh>();
            _cFishes = cFishObjects.Select(x => (GH_CFish)x).ToList();
            var modelMeshes = _cFishes.Select(x => x.Value.ModelMesh).ToList();
            while (true)
            {
                Vector3d yVec = plane.YAxis * (yInterval * countY);
                for (int countX = 0; countX < xNum; countX++)
                {
                    int index = countX + xNum * countY;
                    if (index == _cFishes.Count)
                    {
                        remainMesh = false;
                        break;
                    }

                    Vector3d xVec = plane.XAxis * (xInterval * countX);
                    if (modelMeshes[index] != null)
                    {
                        Mesh mesh = modelMeshes[index];
                        Point3d modelMinPt = mesh.GetBoundingBox(Plane.WorldXY).Min;
                        mesh.Rotate(Vector3d.VectorAngle(Vector3d.XAxis, plane.XAxis), Vector3d.ZAxis, modelMinPt);
                        mesh.Translate(xVec + yVec + new Vector3d(plane.Origin) - new Vector3d(modelMinPt));
                        translatedMeshes.Append(new GH_Mesh(mesh), new GH_Path(0, _cFishes[index].Value.ModelNumber));
                        _tagPlanes.Add(new Plane(modelMinPt - plane.YAxis * 2 * _size, plane.XAxis, plane.YAxis));
                    }
                }
                if (!remainMesh)
                {
                    break;
                }
                countY++;
            }

            DA.SetDataTree(0, translatedMeshes);
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            base.DrawViewportWires(args);
            for (int i = 0; i < _cFishes.Count; i++)
            {
                var text3d = new Text3d(_cFishes[i].ToString(), _tagPlanes[i], _size);
                args.Display.Draw3dText(text3d, Color.Black);
            }
        }

        protected override Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("46E05DFF-8EFD-418A-AC5B-7C9F24559B2E");
    }
}

