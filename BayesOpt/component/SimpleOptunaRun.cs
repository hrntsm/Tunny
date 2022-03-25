using System;
using System.Security.Cryptography;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Python.Runtime;

namespace BayesOpt
{
    public class SimpleOptunaRunComponent : GH_Component
    {
        public SimpleOptunaRunComponent()
          : base("SimpleBayesOpt Component", "Simple",
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

            if (active)
            {
                using (Py.GIL())
                {
                    PyModule ps = Py.CreateScope();
                    ps.Import("optuna");
                    ps.Exec(
                        "def objective(trial):\n" +
                        "    x = trial.suggest_float('x', -2, 2)\n" +
                        "    y = trial.suggest_float('y', -2, 2)\n" +
                        "    return (1 + (x + y + 1)**2 * (19 - 14* x + 3* x **2 -14* y + 6* x * y + 3* y **2)) * (30 + (2* x - 3* y) **2 * (18 - 32* x + 12* x **2 + 48* y - 36* x * y + 27* y **2))\n" +

                        "sampler = optuna.samplers.TPESampler(seed=10)\n" +
                        "study = optuna.create_study(sampler=sampler)\n" +
                        "study.optimize(objective, n_trials=300)\n" +

                        "fig = optuna.visualization.plot_optimization_history(study)\n" +
                        "fig.show()\n"
                    );
                }
            }
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("211c15ee-375f-409b-a92b-5f0c7e5c5b54");
    }
}