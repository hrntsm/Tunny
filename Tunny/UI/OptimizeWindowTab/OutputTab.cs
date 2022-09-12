using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Tunny.Handler;
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
            outputStopButton.Enabled = true;
            Output.Mode = mode;
            Output.Settings = _settings;
            Output.StudyName = studyNameTextBox.Text;
            Output.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
            bool result = SetOutputIndices(mode);
            if (result)
            {
                outputResultBackgroundWorker.RunWorkerAsync(_component);
            }
        }

        private bool SetOutputIndices(OutputMode mode)
        {
            bool result = true;
            var indices = new List<int>();
            switch (mode)
            {
                case OutputMode.ParatoSolutions:
                    Output.Indices = new[] { -1 };
                    break;
                case OutputMode.AllTrials:
                    Output.Indices = new[] { -10 };
                    break;
                case OutputMode.ModelNumber:
                    result = ParseModelNumberInput(ref indices);
                    if (result)
                    {
                        Output.Indices = indices.ToArray();
                    }
                    break;
                case OutputMode.ReflectToSliders:
                    result = ParseModelNumberInput(ref indices);
                    if (result)
                    {
                        CheckIndicesLength(indices.ToArray());
                        Output.Indices = new[] { indices[0] };
                    }
                    break;
                default:
                    throw new ArgumentException("Unsupported output mode.");
            }
            return result;
        }

        private bool ParseModelNumberInput(ref List<int> indices)
        {
            bool result = true;
            try
            {
                indices = outputModelNumTextBox.Text.Split(',').Select(int.Parse).ToList();
            }
            catch (Exception)
            {
                TunnyMessageBox.Show("The model number format of the input is incorrect. \nPlease use a comma separator as follows.\n\"1,2,3\"", "Tunny");
                result = false;
            }
            return result;
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
            Output.IsForcedStopOutput = true;
            switch (Output.Mode)
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

            outputStopButton.Enabled = false;
            Output.IsForcedStopOutput = false;
        }

        private void OutputProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            outputProgressBar.Value = e.ProgressPercentage;
            outputProgressBar.Update();
        }
    }
}
