using System;
using System.Collections.Generic;
using System.Security.Cryptography;

using Grasshopper.Kernel;

namespace BayesOpt
{
    public class BayesOptComponent : GH_Component
    {
        public BayesOptComponent()
          : base("BayesOpt Component", "Nickname",
            "Description of component",
            "NaUtil", "BayesOpt")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Variables", "var", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("Objectives", "obj", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("run", "run", "", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool run = false;
            var variables = new List<double>();
            var objectives = new List<double>();
            if (!DA.GetDataList(0, variables)) return;
            if (!DA.GetDataList(1, objectives)) return;
            if (!DA.GetData(2, ref run)) { return; }

            if (run)
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    var slider = Params.Input[0].Sources[i];
                    if (slider.GetType() == typeof(Grasshopper.Kernel.Special.GH_NumberSlider))
                    {
                        Grasshopper.Kernel.Special.GH_NumberSlider s = (Grasshopper.Kernel.Special.GH_NumberSlider)slider;
                        decimal min = s.Slider.Minimum;
                        decimal max = s.Slider.Maximum;
                        var random = new Random((i + 1) * CreateRandomSeed());

                        decimal val = min + (max - min) * random.Next(0, 100) / 100;
                        s.TrySetSliderValue(val);
                    }
                }
            }
        }

        private int CreateRandomSeed()
        {
            var bs = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bs);
            }
            return BitConverter.ToInt32(bs, 0);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("d7ea83bc-c0a2-4f8e-85b1-e5bccecdef35");
    }
}