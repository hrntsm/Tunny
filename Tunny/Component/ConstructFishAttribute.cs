using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;

using Tunny.Type;

namespace Tunny.Component
{
    public class ConstructFishAttribute : GH_Component, IGH_VariableParameterComponent
    {
        private readonly string _geomDescription = "Connect model geometries here. Not required. Large size models are not recommended as it affects the speed of analysis.\"Geometry\" is a special nickname that is visualized with the optimization results. Do not change it.";
        private readonly string _attrDescription = "Attributes to each trial. Attribute name will be the nickname of the input, so change it to any value.";
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public ConstructFishAttribute()
          : base("Construct Fish Attribute", "ConstrFA",
            "Construct Fish Attribute.",
            "Params", "Tunny")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "Geometry", _geomDescription, GH_ParamAccess.list);
            pManager.AddGenericParameter("Attr1", "Attr1", _attrDescription, GH_ParamAccess.list);
            pManager.AddGenericParameter("Attr2", "Attr2", _attrDescription, GH_ParamAccess.list);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_FishAttribute(), "Attributes", "Attrs", "Attributes to each Trial", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int paramCount = Params.Input.Count;
            var dict = new Dictionary<string, object>();

            for (int i = 0; i < paramCount; i++)
            {
                var list = new List<object>();
                if (!DA.GetDataList(i, list))
                {
                    continue;
                }
                string key = Params.Input[i].NickName;
                dict.Add(key, list);
            }

            DA.SetData(0, dict);
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input)
            {
                if (index == 0)
                {
                    var p = new Param_Geometry();
                    p.Name = p.NickName = "Geometry";
                    p.Description = _geomDescription;
                    p.Access = GH_ParamAccess.list;
                    p.Optional = true;
                    return p;
                }
                else
                {
                    var p = new Param_GenericObject();
                    p.Name = p.NickName = $"Attr{index}";
                    p.Description = _attrDescription;
                    p.Access = GH_ParamAccess.list;
                    p.Optional = true;
                    return p;
                }
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
            foreach (IGH_Param param in Params)
            {
                param.ObjectChanged -= ParamChangedHandler;
                param.ObjectChanged += ParamChangedHandler;
            }
        }

        private void ParamChangedHandler(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
        {
            if (e.Type == GH_ObjectEventType.NickName)
            {
                if (sender.NickName.Length == 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Attribute nickname length must be longer than 0.");
                }
                else
                {
                    ExpireSolution(true);
                }
            }
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("0E66E2E8-6A97-45E0-93DF-2251C3949B7D");
    }
}
