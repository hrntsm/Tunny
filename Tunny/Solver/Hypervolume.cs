using System.Collections.Generic;
using System.Linq;

using Python.Runtime;

namespace Tunny.Solver
{
    public static class Hypervolume
    {
        public static dynamic CreateFigure(dynamic optuna, dynamic study)
        {

            var trials = (dynamic[])study.trials;
            int objectivesCount = ((double[])trials[0].values).Length;
            var trialValues = new List<double[]>();
            foreach (dynamic trial in trials)
            {
                trialValues.Add((double[])trial.values);
            }
            double[] maxObjectiveValues = new double[objectivesCount];
            for (int i = 0; i < objectivesCount; i++)
            {
                maxObjectiveValues[i] = trialValues.Select(v => v[i]).Max();
            }

            PyList hvs = ComputeHypervolume(optuna, trials, maxObjectiveValues, out PyList trialNumbers);
            return CreateHypervolumeFigure(trials, hvs, trialNumbers);
        }

        private static PyList ComputeHypervolume(dynamic optuna, dynamic[] trials, double[] maxObjectiveValues, out PyList trialNumbers)
        {
            dynamic np = Py.Import("numpy");

            var hvs = new PyList();
            var rpObj = new PyList();
            trialNumbers = new PyList();

            foreach (double max in maxObjectiveValues)
            {
                rpObj.Append(new PyFloat(max));
            }
            dynamic referencePoint = np.array(rpObj);

            dynamic wfg = optuna._hypervolume.WFG();
            for (int i = 1; i < trials.Length + 1; i++)
            {
                var vector = new PyList();
                for (int j = 0; j < i; j++)
                {
                    vector.Append(trials[j].values);
                }
                hvs.Append(wfg.compute(np.array(vector), referencePoint));
                trialNumbers.Append(new PyInt(i));
            }
            return hvs;
        }

        private static dynamic CreateHypervolumeFigure(dynamic[] trials, PyList hvs, PyList trialNumbers)
        {
            dynamic go = Py.Import("plotly.graph_objects");

            var plotItems = new PyDict();
            plotItems.SetItem("x", trialNumbers);
            plotItems.SetItem("y", hvs);

            var plotRange = new PyDict();
            var rangeObj = new PyObject[] { new PyFloat(0), new PyFloat(trials.Length + 1) };
            plotRange.SetItem("range", new PyList(rangeObj));

            dynamic fig = go.Figure();
            fig.add_trace(go.Scatter(plotItems, name: "Hypervolume"));
            fig.update_layout(xaxis: plotRange);
            fig.update_xaxes(title_text: "#Trials");
            fig.update_yaxes(title_text: "Hypervolume");

            return fig;
        }
    }
}
