using System;
using System.Security.Cryptography;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace BayesOpt
{
    public class BayesOptComponent : GH_Component
    {
        private int _i = 0;
        private double _cacheValue;
        private GH_Document _doc;

        private SolverState _state = SolverState.Inactive;

        public BayesOptComponent()
          : base("BayesOpt Component", "Nickname",
            "Description of component",
            "NaUtil", "BayesOpt")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Active", "Active", "", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Reset", "Reset", "", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool active = false;
            bool reset = false;
            if (!DA.GetData("Active", ref active)) { return; }
            if (!DA.GetData("Reset", ref reset)) { return; }

            _doc = OnPingDocument();
            _doc.SolutionEnd -= OnSolutionEnd;

            if (reset)
            {
                _cacheValue = 0;
                _state = SolverState.Running;
            }

            if (!active)
            {
                _cacheValue = 0;
                _state = SolverState.Inactive;
                Message = _state.ToString();
                return;
            }

            _doc.SolutionEnd += OnSolutionEnd;

            if (_state == SolverState.Inactive)
            {
                _cacheValue = 0;
                _state = SolverState.Running;
                Message = _state.ToString();
            }
        }

        private void OnSolutionEnd(object sender, GH_SolutionEventArgs e)
        {
            if (_state == SolverState.Inactive) return;
            if (_state == SolverState.Completed) return;

            _cacheValue = GetResult();
            if (_cacheValue > 10 && _cacheValue < 11)
            {
                // We're done.
                _state = SolverState.Completed;
                Message = "Completed";
                return;
            }

            foreach (IGH_DocumentObject obj in _doc.Objects)
            {
                if (obj is Grasshopper.Kernel.Special.GH_Group group && group.NickName == "Variables")
                {
                    foreach (IGH_DocumentObject gObj in group.Objects())
                    {
                        if (gObj is Grasshopper.Kernel.Special.GH_NumberSlider s && _state != SolverState.Completed)
                        {
                            decimal min = s.Slider.Minimum;
                            decimal max = s.Slider.Maximum;
                            var random = new Random(_i++ * CreateRandomSeed());

                            decimal val = min + (max - min) * random.Next(0, 100) / 100;
                            s.SetSliderValue(val);
                        }
                    }
                }
            }

            _doc.ScheduleSolution(5, Callback);
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

        private double GetResult()
        {
            foreach (IGH_DocumentObject obj in _doc.Objects)
            {
                if (!(obj is Grasshopper.Kernel.Parameters.Param_Number param))
                {
                    continue;
                }
                if (param.NickName == "Objectives")
                {
                    if (!(param.VolatileData is GH_Structure<GH_Number> data))
                    {
                        return double.NaN;
                    }

                    foreach (GH_Number number in data.AllData(true))
                    {
                        if (number != null)
                        {
                            return number.Value;
                        }
                    }

                    return double.NaN;
                }
            }
            return double.NaN;
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("d7ea83bc-c0a2-4f8e-85b1-e5bccecdef35");
    }
}