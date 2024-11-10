using Optuna.Util;

using Python.Runtime;

namespace Optuna.Visualization
{
    public static class Visualization
    {
        public static PlotlyFigure PlotSlice(dynamic study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_slice.py"));
            dynamic pyPlotSlice = ps.Get("plot_slice");
            dynamic fig = pyPlotSlice(study, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotParetoFront(dynamic study, string[] objectiveNames, int[] objectiveIndices, bool includeDominatedTrials)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_pareto_front.py"));
            dynamic pyPlotParetoFront = ps.Get("plot_pareto_front");
            dynamic fig = pyPlotParetoFront(study, objectiveNames, objectiveIndices, includeDominatedTrials);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotParamImportances(dynamic study, string objectiveName, int objectiveIndex, string[] variableNames, string evaluator)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_importances.py"));
            dynamic pyPlotParamImportances = ps.Get("plot_importances");
            var pyList = new PyList();
            foreach (string item in variableNames)
            {
                pyList.Append(new PyString(item));
            }
            dynamic fig = pyPlotParamImportances(study, objectiveName, objectiveIndex, pyList, evaluator);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotParallelCoordinate(dynamic study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_parallel_coordinate.py"));
            dynamic pyPlotParallelCoordinate = ps.Get("plot_parallel_coordinate");
            dynamic fig = pyPlotParallelCoordinate(study, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotOptimizationHistory(dynamic study, string objectiveName, int objectiveIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_optimization_history.py"));
            dynamic pyPlotOptimizationHistory = ps.Get("plot_optimization_history");
            dynamic fig = pyPlotOptimizationHistory(study, objectiveName, objectiveIndex);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotEDF(dynamic study, string objectiveName, int objectiveIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_edf.py"));
            dynamic pyPlotEDF = ps.Get("plot_edf");
            dynamic fig = pyPlotEDF(study, objectiveName, objectiveIndex);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotContour(dynamic study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_contour.py"));
            dynamic pyPlotContour = ps.Get("plot_contour");
            dynamic fig = pyPlotContour(study, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotHypervolume(dynamic study, double[] referencePoint)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_hypervolume.py"));
            dynamic pyPlotHypervolume = ps.Get("plot_hypervolume");
            dynamic fig = pyPlotHypervolume(study, referencePoint);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotClustering(dynamic study, int nClusters, int[] objectiveIndex, int[] variableIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_clustering.py"));
            dynamic pyPlotClustering = ps.Get("plot_clustering");
            dynamic fig = pyPlotClustering(study, nClusters, objectiveIndex, variableIndex);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure TruncateParetoFrontPlotHover(dynamic study, PlotlyFigure fig)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_pareto_front.py"));
            dynamic truncate = ps.Get("truncate");
            return new PlotlyFigure(truncate(fig.PyFigure, study));
        }

        public static PlotlyFigure PlotRank(dynamic study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_rank.py"));
            dynamic pyPlotRank = ps.Get("plot_rank");
            dynamic fig = pyPlotRank(study, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotTimeline(dynamic study)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_timeline.py"));
            dynamic pyPlotTimeline = ps.Get("plot_timeline");
            dynamic fig = pyPlotTimeline(study);
            return new PlotlyFigure(fig);
        }
    }
}
