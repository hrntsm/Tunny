using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Tunny.Type;
using Tunny.Util;

namespace Tunny.Component
{
    public class ConstructFishEgg : GH_Component
    {
        public Dictionary<string, Egg> Eggs { get; private set; } = new Dictionary<string, Egg>();
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public ConstructFishEgg()
          : base("Construct Fish Egg", "ConstrEgg",
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
            pManager.AddParameter(new Param_FishEgg(), "Egg", "Egg", "These eggs are enqueued for optimization and become fish.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool lay = false;
            bool clear = false;
            if (!DA.GetData(1, ref lay)) { return; }
            if (!DA.GetData(2, ref clear)) { return; }

            if (clear)
            {
                Eggs.Clear();
                return;
            }

            if (lay)
            {
                var ghIO = new GrasshopperInOut(this, true);
                List<Variable> variables = ghIO.Variables;

                bool isContain = false;
                if (Eggs.Count > 0)
                {
                    int sameValueCount = 0;
                    foreach (Variable variable in variables)
                    {
                        if (Eggs.TryGetValue(variable.NickName, out Egg egg) && egg.Values.Contains(variable.Value))
                        {
                            sameValueCount++;
                        }
                    }
                    isContain = sameValueCount == Eggs.Count;
                }

                if (!isContain)
                {
                    foreach (Variable variable in variables)
                    {
                        if (Eggs.TryGetValue(variable.NickName, out Egg egg))
                        {
                            egg.Values.Add(variable.Value);
                        }
                        else
                        {
                            Eggs.Add(variable.NickName, new Egg(variable));
                        }
                    }
                }
            }

            DA.SetData(0, Eggs);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("00CC0C86-687F-4A28-93CF-30A1E361A7D5");
    }

    public class Egg
    {
        public bool IsInteger { get; set; }
        public string NickName { get; set; }
        public List<double> Values { get; set; }

        public Egg(Variable variable)
        {
            IsInteger = variable.IsInteger;
            NickName = variable.NickName;
            Values = new List<double> { variable.Value };
        }

        public Dictionary<string, object> ToEnqueueDictionary()
        {
            return new Dictionary<string, object>
            {
                {"IsInteger", IsInteger},
                {"NickName", NickName},
                {"Value", Values},
            };
        }

    }
}
