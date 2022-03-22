using System.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

using GH_IO.Serialization;

using Grasshopper.Kernel;

namespace BayesOpt
{
    public class BayesOptComponent : GH_Component
    {
        int _i = 0;
        private SolverState _state = SolverState.Inactive;

        public BayesOptComponent()
          : base("BayesOpt Component", "Nickname",
            "Description of component",
            "NaUtil", "BayesOpt")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("run", "run", "", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool run = false;
            if (!DA.GetData("run", ref run)) { return; }

            GH_Document doc = OnPingDocument();
            doc.SolutionEnd -= OnSolutionEnd;

            if (run)
            {
                Message = _state.ToString();
                foreach (IGH_DocumentObject obj in doc.Objects)
                {
                    if (obj is Grasshopper.Kernel.Special.GH_Group group && obj.NickName == "Variables")
                    {
                        foreach (IGH_DocumentObject gObj in group.Objects())
                        {
                            if (gObj is Grasshopper.Kernel.Special.GH_NumberSlider s)
                            {
                                decimal min = s.Slider.Minimum;
                                decimal max = s.Slider.Maximum;
                                var random = new Random(_i++ * CreateRandomSeed());

                                decimal val = min + (max - min) * random.Next(0, 100) / 100;
                                s.TrySetSliderValue(val);
                            }
                        }
                    }
                }
            }
            doc.SolutionEnd += OnSolutionEnd;
        }
        private void OnSolutionEnd(object sender, GH_SolutionEventArgs e)
        {
            OnPingDocument().ScheduleSolution(5, Callback);
        }

        private void Callback(GH_Document doc)
        {
            ExpireSolution(false);
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

        private enum SolverState
        {
            Inactive,
            Running,
            Completed
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("d7ea83bc-c0a2-4f8e-85b1-e5bccecdef35");
    }
}
