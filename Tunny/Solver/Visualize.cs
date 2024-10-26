using System;
using System.IO;
using System.Windows;

using Optuna.Study;
using Optuna.Visualization;

using Python.Runtime;

using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.UI;

namespace Tunny.Solver
{
    public class Visualize : PythonInit
    {
        private readonly TSettings _settings;
        private readonly bool _hasConstraint;

        public Visualize(TSettings settings, bool hasConstraint)
        {
            TLog.MethodStart();
            _settings = settings;
            _hasConstraint = hasConstraint;
        }

        public string Plot(Plot pSettings)
        {
            TLog.MethodStart();
            string htmlPath = string.Empty;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic storage = _settings.Storage.CreateNewOptunaStorage(false);
                dynamic study = Study.LoadStudy(optuna, storage, pSettings.TargetStudyName);
                if (study == null)
                {
                    return string.Empty;
                }

                try
                {
                    Visualization visualize = CreateFigure(study, pSettings);
                    htmlPath = FigureActions(visualize, pSettings);
                }
                catch (Exception)
                {
                    string message = "This visualization type is not supported in this study case.";
                    TunnyMessageBox.Show(message, "Tunny", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            PythonEngine.Shutdown();
            return htmlPath;
        }

        private Visualization CreateFigure(dynamic study, Plot pSettings)
        {
            TLog.MethodStart();
            var visualize = new Visualization(study);
            switch (pSettings.PlotTypeName)
            {
                case "contour":
                    visualize.Contour(pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
                    break;
                case "EDF":
                    visualize.EDF(pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
                    break;
                case "optimization history":
                    visualize.OptimizationHistory(pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
                    break;
                case "parallel coordinate":
                    visualize.ParallelCoordinate(pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
                    break;
                case "param importances":
                    visualize.ParamImportances(pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
                    break;
                case "pareto front":
                    visualize.ParetoFront(pSettings.TargetObjectiveName, pSettings.TargetObjectiveIndex, _hasConstraint, pSettings.IncludeDominatedTrials);
                    visualize.TruncateParetoFrontPlotHover();
                    break;
                case "slice":
                    visualize.Slice(pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName[0]);
                    break;
                case "hypervolume":
                    visualize.Hypervolume();
                    break;
                case "clustering":
                    visualize.Clustering(pSettings.ClusterCount, pSettings.TargetObjectiveIndex, pSettings.TargetVariableIndex);
                    break;
                default:
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny");
                    break;
            }
            return visualize;
        }

        private static string FigureActions(Visualization visualize, Plot pSettings)
        {
            TLog.MethodStart();
            if (visualize.HasFigure && pSettings.PlotActionType == PlotActionType.Show)
            {
                return SaveFigure(visualize, pSettings.PlotTypeName, true);
            }
            else if (visualize.HasFigure && pSettings.PlotActionType == PlotActionType.Save)
            {
                return SaveFigure(visualize, pSettings.PlotTypeName, false);
            }
            else
            {
                throw new ArgumentException("This visualization type is not supported in this study case.");
            }
        }

        private static string SaveFigure(Visualization visualize, string name, bool forShow)
        {
            TLog.MethodStart();
            string savePath = string.Empty;
            if (forShow)
            {
                savePath = Path.Combine(TEnvVariables.TmpDirPath, name + ".html");
            }
            else
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = name + ".html",
                    Filter = @"HTML file(*.html)|*.html",
                    Title = @"Save"
                };
                if (sfd.ShowDialog() == true)
                {
                    savePath = sfd.FileName;
                }

            }
            if (string.IsNullOrEmpty(savePath))
            {
                return string.Empty;
            }

            visualize.SaveHtml(savePath);
            return savePath;
        }
    }
}
