using System;

using Grasshopper.Kernel;

using Python.Runtime;

namespace BayesOpt
{
    public class InputRunComponent : GH_Component
    {
        public InputRunComponent()
          : base("InputRun Component", "Numpy",
              "Python.NET Component",
              "NaUtil", "BayesOpt")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("python code", "code", "a", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Result", "res", "Description", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string a = string.Empty;
            double res = 0;
            if (!DA.GetData(0, ref a)) return;

            using (Py.GIL())
            {
                PyModule ps = Py.CreateScope();
                ps.Exec(a);
                res = ps.Get<double>("res");
            }

            DA.SetData(0, res);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("60d9f09e-ae2f-425d-9a85-9a8aa6a8e3af");
    }
}