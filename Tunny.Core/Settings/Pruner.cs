using System;
using System.Diagnostics;

using Optuna.Pruner;

using Python.Runtime;

using Tunny.Core.Util;

namespace Tunny.Core.Settings
{
    public class Pruner
    {
        public PrunerType Type { get; set; } = PrunerType.None;
        public string ReporterPath { get; set; } = string.Empty;
        public string StopperPath { get; set; } = string.Empty;
        public string[] ReporterInput { get; set; } = Array.Empty<string>();
        public string[] StopperInput { get; set; } = Array.Empty<string>();
        public HyperbandPruner Hyperband { get; set; } = new HyperbandPruner();
        public MedianPruner Median { get; set; } = new MedianPruner();
        public NopPruner Nop { get; set; } = new NopPruner();
        public PatientPruner Patient { get; set; } = new PatientPruner();
        public PercentilePruner Percentile { get; set; } = new PercentilePruner();
        public SuccessiveHalvingPruner SuccessiveHalving { get; set; } = new SuccessiveHalvingPruner();
        public ThresholdPruner Threshold { get; set; } = new ThresholdPruner();
        public WilcoxonPruner Wilcoxon { get; set; } = new WilcoxonPruner();

        public void Report()
        {
            var reporter = new Process();
            reporter.StartInfo.FileName = ReporterPath;
            reporter.StartInfo.Arguments = string.Join(" ", ReporterInput);
            reporter.Start();
        }

        public void Stop()
        {
            var stopper = new Process();
            stopper.StartInfo.FileName = StopperPath;
            stopper.StartInfo.Arguments = string.Join(" ", StopperInput);
            stopper.Start();
        }

        public dynamic ToPython()
        {
            TLog.MethodStart();
            dynamic optunaPruner;
            dynamic optuna = Py.Import("optuna");
            switch (Type)
            {
                case PrunerType.Hyperband:
                    optunaPruner = Hyperband.ToPython(optuna);
                    break;
                case PrunerType.Median:
                    optunaPruner = Median.ToPython(optuna);
                    break;
                case PrunerType.Nop:
                    optunaPruner = Nop.ToPython(optuna);
                    break;
                case PrunerType.Patient:
                    optunaPruner = Patient.ToPython(optuna);
                    break;
                case PrunerType.Percentile:
                    optunaPruner = Percentile.ToPython(optuna);
                    break;
                case PrunerType.SuccessiveHalving:
                    optunaPruner = SuccessiveHalving.ToPython(optuna);
                    break;
                case PrunerType.Threshold:
                    optunaPruner = Threshold.ToPython(optuna);
                    break;
                case PrunerType.Wilcoxon:
                    optunaPruner = Wilcoxon.ToPython(optuna);
                    break;
                case PrunerType.None:
                    optunaPruner = null;
                    break;
                default:
                    throw new ArgumentException("Invalid pruner type.");
            }

            return optunaPruner;
        }
    }

    public enum PrunerType
    {
        Hyperband,
        Median,
        Nop,
        Patient,
        Percentile,
        SuccessiveHalving,
        Threshold,
        Wilcoxon,
        None = -1
    }
}
