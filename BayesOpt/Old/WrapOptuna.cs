using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using Python.Runtime;

namespace BayesOpt
{
    public class WrapOptunaComponent : GH_Component
    {
        public WrapOptunaComponent()
          : base("Wrap Optuna Component", "Numpy",
              "Python.NET Component",
              "NaUtil", "BayesOpt")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("a", "a", "a", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("res", "r", "Description", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool a = false;
            if (!DA.GetData(0, ref a)) return;
            var res = new List<double>();

            if (a)
            {
                res = OptunaRun();
            }

            DA.SetDataList(0, res);
        }

        private List<double> OptunaRun()
        {
            var res = new List<double>();

            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study = optuna.create_study();
                int nTrials = 30;

                for (int i = 0; i < nTrials; i++)
                {
                    dynamic trial = study.ask();
                    dynamic x = trial.suggest_uniform("x", -10, 10);
                    dynamic y = (x - 2) * (x - 2);
                    study.tell(trial, y);
                }
                res.Add((double)study.best_value);
                res.Add((double)study.best_params["x"]);

                dynamic vis = optuna.visualization.plot_optimization_history(study);
                vis.show();
            }
            return res;
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("6252f61d-b52f-4243-a298-ba2d8a8e12fd");
    }
}