using System;

using Optuna.Storage;
using Optuna.Util;

using Python.Runtime;

namespace Optuna.Visualization
{
    public class Visualization
    {
        public bool HasFigure
        {
            get
            { return _fig != null; }
        }

        private readonly dynamic _study;
        private dynamic _fig;

        public Visualization(dynamic study)
        {
            _study = study;
        }

        public Visualization(string storagePath, string studyName)
        {
            IOptunaStorage storage = StorageHelper.GetStorage(storagePath);
            _study = Study.Study.LoadStudy(Py.Import("optuna"), storage, studyName);
        }

        public void Slice(string objectiveName, int objectiveIndex, string variableName)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_slice.py"));
            dynamic visualize = ps.Get("plot_slice");
            _fig = visualize(_study, objectiveName, objectiveIndex, variableName);
        }

        public void ParetoFront(string[] objectiveNames, int[] objectiveIndices, bool hasConstraint, bool includeDominatedTrials)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_pareto_front.py"));
            dynamic visualize = ps.Get("plot_pareto_front");
            _fig = visualize(_study, objectiveNames, objectiveIndices, hasConstraint, includeDominatedTrials);
        }

        public void ParamImportances(string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_importances.py"));
            dynamic visualize = ps.Get("plot_importances");
            var pyList = new PyList();
            foreach (string item in variableNames)
            {
                pyList.Append(new PyString(item));
            }
            _fig = visualize(_study, objectiveName, objectiveIndex, pyList);
        }

        public void ParallelCoordinate(string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_parallel_coordinate.py"));
            dynamic visualize = ps.Get("plot_parallel_coordinate");
            _fig = visualize(_study, objectiveName, objectiveIndex, variableNames);
        }

        public void OptimizationHistory(string objectiveName, int objectiveIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_optimization_history.py"));
            dynamic visualize = ps.Get("plot_optimization_history");
            _fig = visualize(_study, objectiveName, objectiveIndex);
        }

        public void EDF(string objectiveName, int objectiveIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_edf.py"));
            dynamic visualize = ps.Get("plot_edf");
            _fig = visualize(_study, objectiveName, objectiveIndex);
        }

        public void Contour(string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_contour.py"));
            dynamic visualize = ps.Get("plot_contour");
            _fig = visualize(_study, objectiveName, objectiveIndex, variableNames);
        }

        public void Hypervolume()
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_hypervolume.py"));
            dynamic visualize = ps.Get("plot_hypervolume");
            _fig = visualize(_study);
        }

        public void Clustering(int nClusters, int[] objectiveIndex, int[] variableIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_clustering.py"));
            dynamic visualize = ps.Get("plot_clustering");
            _fig = visualize(_study, nClusters, objectiveIndex, variableIndex);
        }

        public void TruncateParetoFrontPlotHover()
        {
            CheckPlotCreated();
            PyModule ps = Py.CreateScope();
            ps.Exec(ReadFileFromResource.Text("Optuna.Visualization.Python.plot_pareto_front.py"));
            dynamic truncate = ps.Get("truncate");
            _fig = truncate(_fig, _study);
        }

        private void CheckPlotCreated()
        {
            if (_fig == null)
            {
                throw new InvalidOperationException("No plot has been created yet.");
            }
        }

        public string Show()
        {
            CheckPlotCreated();
            return _fig.to_html();
        }

        public void SaveHtml(string path)
        {
            CheckPlotCreated();
            _fig.write_html(path);
        }
    }
}
