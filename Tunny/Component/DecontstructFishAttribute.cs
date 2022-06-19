using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

using Tunny.Type;

namespace Tunny.Component
{
    public class DeconstructFishAttribute : GH_Component, IGH_VariableParameterComponent
    {
        private int _outputCount = 0;
        private List<string> _keys = new List<string>();
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public DeconstructFishAttribute()
          : base("Deconstruct Fish Attribute", "DeconFA",
              "Deconstruct Fish Attribute.",
              "Params", "Tunny")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_FishAttribute(), "Attributes", "Attrs", "Attributes to each Trial", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!DA.GetDataTree(0, out GH_Structure<GH_FishAttribute> fishAttributeStructure)) { return; }

            var nicknames = Params.Output.Select(x => x.NickName).ToList();
            var outputValues = new Dictionary<string, GH_Structure<IGH_Goo>>();
            for (int i = 0; i < fishAttributeStructure.PathCount; i++)
            {
                GH_Path path = fishAttributeStructure.Paths[i];
                Dictionary<string, object> value = fishAttributeStructure.Branches[i][0].Value;

                //FIXME: This process should be done once, but foreach executes it every time.
                _keys = value.Keys.ToList();

                foreach (KeyValuePair<string, object> pair in value)
                {
                    if (nicknames.Contains(pair.Key))
                    {
                        if (!outputValues.ContainsKey(pair.Key))
                        {
                            outputValues.Add(pair.Key, new GH_Structure<IGH_Goo>());
                        }

                        List<IGH_Goo> goo = GetGooFromAttributeObject(pair.Value);
                        foreach (IGH_Goo g in goo)
                        {
                            outputValues[pair.Key].Append(g, path);
                        }
                    }
                }
            }

            if (Params.Output.Count == 0)
            {
                return;
            }

            for (int i = 0; i < Params.Output.Count; i++)
            {
                foreach (string nickName in nicknames)
                {
                    if (Params.Output[i].NickName == nickName)
                    {
                        DA.SetDataTree(i, outputValues[nickName]);
                    }
                }
            }
        }

        private List<IGH_Goo> GetGooFromAttributeObject(object value)
        {
            switch (value)
            {
                case List<string> str:
                    return str.Select(s => new GH_String(s)).Cast<IGH_Goo>().ToList();
                case List<GeometryBase> geom:
                    return geom.Select(g => CreateGeometricGoo(g)).Cast<IGH_Goo>().ToList();
                default:
                    return new List<IGH_Goo>();
            }
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

        public bool CanInsertParameter(GH_ParameterSide side, int index) => side != GH_ParameterSide.Input && (Params.Output.Count == 0 || index == Params.Output.Count);

        public bool CanRemoveParameter(GH_ParameterSide side, int index) => (side != GH_ParameterSide.Input) && (index == Params.Output.Count - 1);

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output && _keys.Count() > index)
            {
                var p = new Param_GenericObject();
                p.Name = p.NickName = _keys[index];
                p.MutableNickName = false;
                p.Access = GH_ParamAccess.tree;
                return p;
            }
            else
            {
                return null;
            }
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            if (_outputCount != Params.Output.Count)
            {
                _outputCount = Params.Output.Count;
                _outputNames = Params.Output.Select(x => x.NickName).ToList();
                ExpireSolution(true);
            }
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("D3B7B64A-71BA-41BE-9B2E-B0F459886B36");
    }
}
