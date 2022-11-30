using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Tunny.Type;
using Tunny.Util;

namespace Tunny.Component
{
    public class ConstructFishEgg : GH_Component
    {
        public Dictionary<string, FishEgg> FishEggs { get; private set; } = new Dictionary<string, FishEgg>();
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public ConstructFishEgg()
          : base("Construct Fish Egg", "ConstrFEgg",
            "Construct Fish Egg.",
            "Tunny", "Tunny")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Variables", "Var", "Variables pair to enqueue optimize.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Lay Egg", "Lay", "If true, add an egg", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Clear", "Clear", "If true, clear eggs", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_FishEgg(), "FishEgg", "FishEgg", "These eggs are enqueued for optimization and become fish.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool lay = false;
            bool clear = false;
            if (!DA.GetData(1, ref lay)) { return; }
            if (!DA.GetData(2, ref clear)) { return; }

            if (clear)
            {
                FishEggs.Clear();
                return;
            }

            if (lay)
            {
                var ghIO = new GrasshopperInOut(this, true);
                List<Variable> variables = ghIO.Variables;

                bool isContain = false;
                if (FishEggs.Count > 0)
                {
                    int sameValueCount = 0;
                    foreach (Variable variable in variables)
                    {
                        if (FishEggs.TryGetValue(variable.NickName, out FishEgg egg) && egg.Values.Contains(variable.Value))
                        {
                            sameValueCount++;
                        }
                    }
                    isContain = sameValueCount == FishEggs.Count;
                }

                if (!isContain)
                {
                    foreach (Variable variable in variables)
                    {
                        if (FishEggs.TryGetValue(variable.NickName, out FishEgg egg))
                        {
                            egg.Values.Add(variable.Value);
                        }
                        else
                        {
                            FishEggs.Add(variable.NickName, new FishEgg(variable));
                        }
                    }
                }
            }

            DA.SetData(0, FishEggs);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("00CC0C86-687F-4A28-93CF-30A1E361A7D5");
    }
}
