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
            InitializePythonEngine();
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
                    htmlPath = Path.Combine(TEnvVariables.TmpDirPath, "plot.html");
                    figure.UpdateLayout(new FigureLayout { PaperBgColor = "rgba(0,0,0,0)" });
                    figure.WriteHtml(htmlPath);
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

        private static PlotlyFigure CreateFigure(dynamic study, PlotSettings settings)
        {
            TLog.MethodStart();
            switch (settings.PlotTypeName)
            {
                case "contour":
                    return Visualization.PlotContour(study, settings.TargetObjectiveName[0], settings.TargetObjectiveIndex[0], settings.TargetVariableName);
                case "EDF":
                    return Visualization.PlotEDF(study, settings.TargetObjectiveName[0], settings.TargetObjectiveIndex[0]);
                case "optimization history":
                    return Visualization.PlotOptimizationHistory(study, settings.TargetObjectiveName[0], settings.TargetObjectiveIndex[0]);
                case "parallel coordinate":
                    return Visualization.PlotParallelCoordinate(study, settings.TargetObjectiveName[0], settings.TargetObjectiveIndex[0], settings.TargetVariableName);
                case "param importances":
                    return Visualization.PlotParamImportances(study, settings.TargetObjectiveName[0], settings.TargetObjectiveIndex[0], settings.TargetVariableName, settings.ImportanceEvaluator);
                case "pareto front":
                    PlotlyFigure fig = Visualization.PlotParetoFront(study, settings.TargetObjectiveName, settings.TargetObjectiveIndex, settings.IncludeDominatedTrials);
                    return Visualization.TruncateParetoFrontPlotHover(study, fig);
                case "slice":
                    return Visualization.PlotSlice(study, settings.TargetObjectiveName[0], settings.TargetObjectiveIndex[0], settings.TargetVariableName);
                case "hypervolume":
                    return Visualization.PlotHypervolume(study, settings.ReferencePoint);
                case "clustering":
                    return Visualization.PlotClustering(study, settings.ClusterCount, settings.TargetObjectiveIndex, settings.TargetVariableIndex);
                default:
                    throw new ArgumentException("Invalid plot type");
            }
        }
    }
}
