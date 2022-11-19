using System;
using System.Collections.Generic;
using System.Linq;

using Python.Runtime;

using Tunny.UI;

namespace Tunny.Solver
{
    public static class Hypervolume
    {
        public static dynamic CreateFigure(dynamic study, PlotSettings pSettings)
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

            PyList hvs = ComputeHypervolume(trials, maxObjectiveValues, pSettings, out PyList trialNumbers);
            return CreateHypervolumeFigure(trials, hvs, trialNumbers);
        }

        private static PyList ComputeHypervolume(dynamic[] trials, double[] maxObjectiveValues, PlotSettings pSettings, out PyList trialNumbers)
        {
            dynamic optuna = Py.Import("optuna");
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

        public static double Compute2dHypervolumeRatio(dynamic study)
        {
            var trials = (dynamic[])study.trials;
            var trialValues = new List<double[]>();
            for (int i = 0; i < trials.Length - 1; i++)
            {
                dynamic trial = trials[i];
                trialValues.Add((double[])trial.values);
            }
            double[] maxObjectiveValues = new double[2];
            for (int i = 0; i < 2; i++)
            {
                maxObjectiveValues[i] = trialValues.Select(v => v[i]).Max();
            }

            return ComputeRatio(trials, trialValues.Count - 1, trialValues.Count, maxObjectiveValues);
        }

        private static double ComputeRatio(dynamic[] trials, int baseIndex, int targetIndex, double[] maxObjectiveValues)
        {
            dynamic np = Py.Import("numpy");
            var rpObj = new PyList();

            foreach (double max in maxObjectiveValues)
            {
                rpObj.Append(new PyFloat(max));
            }
            dynamic referencePoint = np.array(rpObj);

            double baseHypervolume = Wfg(trials, baseIndex, referencePoint);
            double targetHypervolume = Wfg(trials, targetIndex, referencePoint);

            return Math.Round(targetHypervolume / baseHypervolume, 3);
        }

        private static double Wfg(dynamic[] trials, int baseIndex, dynamic referencePoint)
        {
            dynamic optuna = Py.Import("optuna");
            dynamic np = Py.Import("numpy");

            var vectors = new PyList();
            dynamic wfg = optuna._hypervolume.WFG();
            for (int j = 0; j < baseIndex; j++)
            {
                var vector = new PyList();
                vector.Append(trials[j].values[0]);
                vector.Append(trials[j].values[1]);
                vectors.Append(vector);
            }
            return (double)wfg.compute(np.array(vectors), referencePoint);
        }
    }
}
