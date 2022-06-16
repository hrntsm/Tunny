using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Display;
using Rhino.Geometry;

using Tunny.Resources;
using Tunny.Type;

namespace Tunny.Component
{
    public class FishMarket : GH_Component
    {
        private readonly List<Plane> _tagPlanes = new List<Plane>();
        private List<GH_Fish> _fishes = new List<GH_Fish>();
        private double _size = 1;
        private Settings _settings = new Settings();

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public FishMarket()
          : base("FishMarket", "FMarket",
            "A place to lay out the solutions we caught.",
            "Params", "Tunny")
        {
        }

        public override void ClearData()
        {
            base.ClearData();
            _tagPlanes.Clear();
            _fishes.Clear();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Fish(), "Fishes", "Fishes", "Fishes caught by the optimization nets.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "Plane", "Plane", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddIntegerParameter("xNum", "xNum", "Number of x.", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("xInterval", "xIntvl", "Interval of x.", GH_ParamAccess.item, 1000);
            pManager.AddNumberParameter("yInterval", "yIntvl", "Interval of y.", GH_ParamAccess.item, 1000);
            pManager.AddNumberParameter("TextSize", "tSize", "Font size.", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometries", "Geom", "Geometries of the fish market.", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var fishObjects = new List<object>();
            var plane = new Plane();
            int xNum = 0;
            double xInterval = 0;
            double yInterval = 0;
            if (!DA.GetDataList(0, fishObjects)) { return; }
            if (!DA.GetData(1, ref plane)) { return; }
            if (!DA.GetData(2, ref xNum)) { return; }
            if (!DA.GetData(3, ref xInterval)) { return; }
            if (!DA.GetData(4, ref yInterval)) { return; }
            if (!DA.GetData(5, ref _size)) { return; }

            _settings = new Settings()
            {
                Plane = plane,
                XNum = xNum,
                XInterval = xInterval,
                YInterval = yInterval
            };
            if (xNum <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "xNum must lager than 0");
                return;
            }
            DA.SetDataTree(0, ArrayedGeometries(fishObjects));
            ExpirePreview(true);
        }

        private GH_Structure<IGH_GeometricGoo> ArrayedGeometries(IEnumerable<object> fishObjects)
        {
            int countY = 0;
            var arrayedGeometries = new GH_Structure<IGH_GeometricGoo>();
            _fishes = fishObjects.Select(x => (GH_Fish)x).ToList();
            var fishGeometries = _fishes.Select(x => x.Value.Geometries).ToList();

            while (true)
            {
                bool remainGeometry = SetGeometryToResultArray(countY, arrayedGeometries, fishGeometries);
                if (!remainGeometry)
                {
                    break;
                }
                countY++;
            }

            return arrayedGeometries;
        }

        private bool SetGeometryToResultArray(int countY, GH_Structure<IGH_GeometricGoo> arrayedGeometries, IReadOnlyList<List<GeometryBase>> fishGeometries)
        {
            Vector3d yVec = _settings.Plane.YAxis * (_settings.YInterval * countY);
            for (int countX = 0; countX < _settings.XNum; countX++)
            {
                int index = countX + _settings.XNum * countY;
                if (index == _fishes.Count)
                {
                    return false;
                }
                Vector3d xVec = _settings.Plane.XAxis * (_settings.XInterval * countX);
                if (fishGeometries[index] != null)
                {
                    Point3d modelMinPt = GetUnionBoundingBoxMinPt(fishGeometries[index]);
                    foreach (GeometryBase geometry in fishGeometries[index])
                    {
                        geometry.Rotate(Vector3d.VectorAngle(Vector3d.XAxis, _settings.Plane.XAxis), Vector3d.ZAxis, modelMinPt);
                        geometry.Translate(xVec + yVec + new Vector3d(_settings.Plane.Origin) - new Vector3d(modelMinPt));
                        arrayedGeometries.Append(CreateGeometricGoo(geometry), new GH_Path(0, _fishes[index].Value.ModelNumber));
                    }
                    _tagPlanes.Add(new Plane(modelMinPt - _settings.Plane.YAxis * 2.5 * _size, _settings.Plane.XAxis, _settings.Plane.YAxis));
                }
            }
            return true;
        }

        private static IGH_GeometricGoo CreateGeometricGoo(GeometryBase geometry)
        {
            switch (geometry)
            {
                case Mesh mesh:
                    return new GH_Mesh(mesh);
                case Curve curve:
                    return new GH_Curve(curve);
                case Brep brep:
                    return new GH_Brep(brep);
                case Surface surface:
                    return new GH_Surface(surface);
                case SubD subD:
                    return new GH_SubD(subD);
                default:
                    throw new Exception("Tunny doesn't handle this type of geometry");
            }
        }

        private static Point3d GetUnionBoundingBoxMinPt(IEnumerable<GeometryBase> geometryBases)
        {
            var minPt = new Point3d(double.MaxValue, double.MaxValue, double.MaxValue);
            foreach (BoundingBox boundingBox in geometryBases.Select(geometry => geometry.GetBoundingBox(Plane.WorldXY)))
            {
                if (boundingBox.Min.X < minPt.X)
                {
                    minPt.X = boundingBox.Min.X;
                }
                if (boundingBox.Min.Y < minPt.Y)
                {
                    minPt.Y = boundingBox.Min.Y;
                }
                if (boundingBox.Min.Z < minPt.Z)
                {
                    minPt.Z = boundingBox.Min.Z;
                }
            }
            return minPt;
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            base.DrawViewportWires(args);
            for (int i = 0; i < _fishes.Count; i++)
            {
                var text3d = new Text3d(_fishes[i].ToString(), _tagPlanes[i], _size);
                args.Display.Draw3dText(text3d, Color.Black);
            }
        }

        protected override Bitmap Icon => Resource.FishMarket;
        public override Guid ComponentGuid => new Guid("46E05DFF-8EFD-418A-AC5B-7C9F24559B2E");

        private class Settings
        {
            public Plane Plane { get; set; }
            public int XNum { get; set; }
            public double XInterval { get; set; }
            public double YInterval { get; set; }
        }
    }
}
