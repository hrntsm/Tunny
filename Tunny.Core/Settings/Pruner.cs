using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using Optuna.Pruner;

using Python.Runtime;

using Tunny.Core.Util;

namespace Tunny.Core.Settings
{
    public class Pruner
    {
        public PrunerType Type { get; set; } = PrunerType.None;
        public string ReporterPath { get; set; } = string.Empty;
        public string ReportFilePath { get; set; } = string.Empty;
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

        public double Evaluate()
        {
            var reporter = new Process();
            reporter.StartInfo.FileName = ReporterPath;
            reporter.StartInfo.Arguments = string.Join(" ", ReporterInput);
            reporter.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            reporter.Start();
            reporter.WaitForExit();

            double value = double.NaN;
            try
            {
                using (var reader = new StreamReader(ReportFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        double.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                    }
                }
                if (double.IsNaN(value))
                {
                    throw new FileLoadException("Failed to load the report file.");
                }
            }
            catch (Exception e)
            {
                TLog.Error(e.Message);
            }
            return value;
        }

        public void Stop()
        {
            var stopper = new Process();
            stopper.StartInfo.FileName = StopperPath;
            stopper.StartInfo.Arguments = string.Join(" ", StopperInput);
            stopper.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            stopper.Start();
            stopper.WaitForExit();
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
