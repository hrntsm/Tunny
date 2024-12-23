using Optuna.Study;
using Optuna.Util;

using Python.Runtime;

namespace Optuna.Visualization
{
    public static class Visualization
    {
        public static PlotlyFigure PlotSlice(StudyWrapper study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_slice.py"));
            dynamic pyPlotSlice = ps.Get("plot_slice");
            dynamic fig = pyPlotSlice(study.PyInstance, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotParetoFront(StudyWrapper study, string[] objectiveNames, int[] objectiveIndices, bool includeDominatedTrials)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_pareto_front.py"));
            dynamic pyPlotParetoFront = ps.Get("plot_pareto_front");
            dynamic fig = pyPlotParetoFront(study.PyInstance, objectiveNames, objectiveIndices, includeDominatedTrials);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotParamImportances(StudyWrapper study, string objectiveName, int objectiveIndex, string[] variableNames, string evaluator)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_importances.py"));
            dynamic pyPlotParamImportances = ps.Get("plot_importances");
            var pyList = new PyList();
            foreach (string item in variableNames)
            {
                pyList.Append(new PyString(item));
            }
            dynamic fig = pyPlotParamImportances(study.PyInstance, objectiveName, objectiveIndex, pyList, evaluator);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotParallelCoordinate(StudyWrapper study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_parallel_coordinate.py"));
            dynamic pyPlotParallelCoordinate = ps.Get("plot_parallel_coordinate");
            dynamic fig = pyPlotParallelCoordinate(study.PyInstance, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotOptimizationHistory(StudyWrapper study, string objectiveName, int objectiveIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_optimization_history.py"));
            dynamic pyPlotOptimizationHistory = ps.Get("plot_optimization_history");
            dynamic fig = pyPlotOptimizationHistory(study.PyInstance, objectiveName, objectiveIndex, false);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotEDF(StudyWrapper study, string objectiveName, int objectiveIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_edf.py"));
            dynamic pyPlotEDF = ps.Get("plot_edf");
            dynamic fig = pyPlotEDF(study.PyInstance, objectiveName, objectiveIndex);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotContour(StudyWrapper study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_contour.py"));
            dynamic pyPlotContour = ps.Get("plot_contour");
            dynamic fig = pyPlotContour(study.PyInstance, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotHypervolume(StudyWrapper study, double[] referencePoint)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_hypervolume.py"));
            dynamic pyPlotHypervolume = ps.Get("plot_hypervolume");
            dynamic fig = pyPlotHypervolume(study.PyInstance, referencePoint);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotClustering(StudyWrapper study, int nClusters, int[] objectiveIndex, int[] variableIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_clustering.py"));
            dynamic pyPlotClustering = ps.Get("plot_clustering");
            dynamic fig = pyPlotClustering(study.PyInstance, nClusters, objectiveIndex, variableIndex);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure TruncateParetoFrontPlotHover(StudyWrapper study, PlotlyFigure fig)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_pareto_front.py"));
            dynamic truncate = ps.Get("truncate");
            return new PlotlyFigure(truncate(fig.PyFigure, study.PyInstance));
        }

        public static PlotlyFigure PlotRank(StudyWrapper study, string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_rank.py"));
            dynamic pyPlotRank = ps.Get("plot_rank");
            dynamic fig = pyPlotRank(study.PyInstance, objectiveName, objectiveIndex, variableNames);
            return new PlotlyFigure(fig);
        }

        public static PlotlyFigure PlotTimeline(StudyWrapper study)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_timeline.py"));
            dynamic pyPlotTimeline = ps.Get("plot_timeline");
            dynamic fig = pyPlotTimeline(study.PyInstance);
            return new PlotlyFigure(fig);
        }
    }
}
