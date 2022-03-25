using System;

using Grasshopper.Kernel;

using Python.Runtime;

namespace BayesOpt
{
    public class NumpyComponent : GH_Component
    {
        public NumpyComponent()
          : base("Numpy Component", "Numpy",
              "Python.NET Component",
              "NaUtil", "BayesOpt")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("a", "a", "a", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("b", "b", "b", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Output", "O", "Description", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double a = 0;
            double b = 0;
            if (!DA.GetData(0, ref a)) return;
            if (!DA.GetData(1, ref b)) return;

            double res = NumpyDotNet.Run(a, b);

            DA.SetData(0, res);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("50D1969B-5064-47E4-BC76-75A27E5FC8F3");
    }

    public class NumpyDotNet
    {
        public static double Run(double a, double b)
        {
            double res = 0;
            using (Py.GIL())
            {
                dynamic np = Py.Import("numpy");
                res = (double)(np.cos(a) + np.sin(b));
            }
            return res;
        }
    }
}