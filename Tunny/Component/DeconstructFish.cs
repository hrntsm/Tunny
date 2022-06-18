using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

using Tunny.Type;
using Tunny.Util;

namespace Tunny.Component
{
    public class DeconstructFish : GH_Component
    {
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public DeconstructFish()
          : base("Deconstruct Fish", "DeconF",
              "Deconstruct Fish.",
              "Params", "Tunny")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Fish(), "Fishes", "Fishes", "Fishes", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Variables", "Vars", "Variables", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Objectives", "Objs", "Objectives", GH_ParamAccess.tree);
            pManager.AddParameter(new Param_FishAttribute(), "Attributes", "Attrs", "Attributes", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var fishObjects = new List<object>();
            if (!DA.GetDataList(0, fishObjects)) { return; }

            var fishes = fishObjects.Select(x => (GH_Fish)x).ToList();

            var variables = new GH_Structure<GH_Number>();
            var objectives = new GH_Structure<GH_Number>();
            var attributes = new GH_Structure<GH_FishAttribute>();

            foreach (GH_Fish fish in fishes)
            {
                Fish value = fish.Value;
                var path = new GH_Path(0, value.ModelNumber);
                foreach (KeyValuePair<string, double> variable in value.Variables)
                {
                    variables.Append(new GH_Number(variable.Value), path);
                }
                foreach (KeyValuePair<string, double> objective in value.Objectives)
                {
                    objectives.Append(new GH_Number(objective.Value), path);
                }
                foreach (KeyValuePair<string, object> attribute in value.Attributes)
                {
                    var attr = new GH_FishAttribute(new Dictionary<string, object>()
                    {
                        { attribute.Key, attribute.Value }
                    });
                    attributes.Append(attr, path);
                }
            }

            DA.SetDataTree(0, variables);
            DA.SetDataTree(1, objectives);
            DA.SetDataTree(2, attributes);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("1D0EA5A9-9499-4D70-8505-6AB4B05889F4");
    }
}
