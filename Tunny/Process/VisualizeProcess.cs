using System;
using System.IO;

using Optuna.Study;
using Optuna.Visualization;

using Python.Runtime;

using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.WPF.Common;

namespace Tunny.Process
{
    internal class VisualizeProcess : PythonInit
    {
        internal string Plot(Storage storage, PlotSettings plotSettings)
        {
            TLog.MethodStart();
            string htmlPath = string.Empty;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optunaStorage = storage.CreateNewOptunaStorage(false);
                dynamic optunaStudy = Study.LoadStudy(optunaStorage, plotSettings.TargetStudyName);
                if (optunaStudy == null)
                {
                    return string.Empty;
                }

                try
                {
                    PlotlyFigure figure = CreateFigure(optunaStudy, plotSettings);
                    htmlPath = Path.Combine(TEnvVariables.TunnyEnvPath, "temp", "plot.html");
                    figure.UpdateLayout(new FigureLayout { PaperBgColor = "rgba(0,0,0,0)" });
                    figure.WtiteHtml(htmlPath);
                }
                catch (Exception)
                {
                    htmlPath = string.Empty;
                    TunnyMessageBox.Error_VisualizationTypeNotSupported();
                }
            }
            PythonEngine.Shutdown();
            return htmlPath;
        }

        private static PlotlyFigure CreateFigure(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            switch (pSettings.PlotTypeName)
            {
                case "contour":
                    return Visualization.PlotContour(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
                case "EDF":
                    return Visualization.PlotEDF(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
                case "optimization history":
                    return Visualization.PlotOptimizationHistory(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
                case "parallel coordinate":
                    return Visualization.PlotParallelCoordinate(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
                case "param importances":
                    return Visualization.PlotParamImportances(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
                case "pareto front":
                    PlotlyFigure fig = Visualization.PlotParetoFront(study, pSettings.TargetObjectiveName, pSettings.TargetObjectiveIndex, pSettings.IncludeDominatedTrials);
                    return Visualization.TruncateParetoFrontPlotHover(study, fig);
                case "slice":
                    return Visualization.PlotSlice(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName[0]);
                case "hypervolume":
                    return Visualization.PlotHypervolume(study);
                case "clustering":
                    return Visualization.PlotClustering(study, pSettings.ClusterCount, pSettings.TargetObjectiveIndex, pSettings.TargetVariableIndex);
                default:
                    throw new ArgumentException("Invalid plot type");
            }
        }
    }
}
