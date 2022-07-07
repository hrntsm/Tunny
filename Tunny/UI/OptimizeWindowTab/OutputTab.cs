using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Tunny.Optimization;
using Tunny.Util;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void OutputParatoSolutionButton_Click(object sender, EventArgs e)
        {
            RunOutputLoop(OutputMode.ParatoSolutions);
        }

        private void OutputAllTrialsButton_Click(object sender, EventArgs e)
        {
            RunOutputLoop(OutputMode.AllTrials);
        }

        private void OutputModelNumberButton_Click(object sender, EventArgs e)
        {
            RunOutputLoop(OutputMode.ModelNumber);
        }

        private void ReflectToSliderButton_Click(object sender, EventArgs e)
        {
            RunOutputLoop(OutputMode.ReflectToSliders);
        }

        private void RunOutputLoop(OutputMode mode)
        {
            OutputLoop.Mode = mode;
            OutputLoop.Settings = _settings;
            OutputLoop.StudyName = studyNameTextBox.Text;
            OutputLoop.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
            int[] indices = outputModelNumTextBox.Text.Split(',').Select(int.Parse).ToArray();
            SetOutputIndices(mode, indices);
            outputResultBackgroundWorker.RunWorkerAsync(_component);
        }

        private static void SetOutputIndices(OutputMode mode, int[] indices)
        {
            switch (mode)
            {
                case OutputMode.ParatoSolutions:
                    OutputLoop.Indices = new[] { -1 };
                    break;
                case OutputMode.AllTrials:
                    OutputLoop.Indices = new[] { -10 };
                    break;
                case OutputMode.ModelNumber:
                    OutputLoop.Indices = indices;
                    break;
                case OutputMode.ReflectToSliders:
                    CheckIndicesLength(indices);
                    OutputLoop.Indices = new[] { indices[0] };
                    break;
                default:
                    throw new ArgumentException("Unsupported output mode.");
            }
        }

        private static void CheckIndicesLength(int[] indices)
        {
            if (indices.Length > 1)
            {
                TunnyMessageBox.Show(
                    "You input multi model numbers, but this function only reflect variables to slider or gene pool to first one.",
                    "Tunny"
                );
            }
        }

        private void OutputStopButton_Click(object sender, EventArgs e)
        {
            if (outputResultBackgroundWorker != null)
            {
                outputResultBackgroundWorker.Dispose();
            }
            switch (OutputLoop.Mode)
            {
                case OutputMode.ParatoSolutions:
                case OutputMode.AllTrials:
                case OutputMode.ModelNumber:
                    _component.ExpireSolution(true);
                    break;
                case OutputMode.ReflectToSliders:
                    var decimalVar = _component.Fishes[0].Variables
                            .Select(x => (decimal)x.Value).ToList();
                    _component.GhInOut.NewSolution(decimalVar);
                    break;
                default:
                    throw new ArgumentException("Unsupported output mode.");
            }
        }

        private void OutputProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            outputProgressBar.Value = e.ProgressPercentage;
            outputProgressBar.Update();
        }
    }
}
