using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Tunny.Enum;
using Tunny.Handler;

namespace Tunny.UI
{
    public partial class OptimizationWindow
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
            OutputLoop.Mode = mode;
            OutputLoop.Settings = _settings;
            OutputLoop.StudyName = outputTargetStudyComboBox.Text;
            if (string.IsNullOrEmpty(OutputLoop.StudyName))
            {
                TunnyMessageBox.Show("Please select study name.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OutputLoop.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
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
                    OutputLoop.Indices = new[] { -1 };
                    break;
                case OutputMode.AllTrials:
                    OutputLoop.Indices = new[] { -10 };
                    break;
                case OutputMode.ModelNumber:
                    result = ParseModelNumberInput(ref indices);
                    if (result)
                    {
                        OutputLoop.Indices = indices.ToArray();
                    }
                    break;
                case OutputMode.ReflectToSliders:
                    result = ParseModelNumberInput(ref indices);
                    if (result)
                    {
                        CheckIndicesLength(indices.ToArray());
                        OutputLoop.Indices = new[] { indices[0] };
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
                result = IncorrectParseModeNumberInputMessage();
            }
            return result;
        }

        private static void CheckIndicesLength(int[] indices)
        {
            if (indices.Length > 1)
            {
                UseFirstModelNumberToReflectMessage();
            }
        }

        private void OutputStopButton_Click(object sender, EventArgs e)
        {
            outputResultBackgroundWorker?.Dispose();
            OutputLoop.IsForcedStopOutput = true;
            switch (OutputLoop.Mode)
            {
                case OutputMode.ParatoSolutions:
                case OutputMode.AllTrials:
                case OutputMode.ModelNumber:
                    _component.ExpireSolution(true);
                    break;
                case OutputMode.ReflectToSliders:
                    throw new NotImplementedException("Reflect to sliders is not implemented.");
                // var decimalVar = _component.Fishes[0].Variables
                //         .Select(x => (decimal)x.Value).ToList();
                // _component.GhInOut.NewSolution(decimalVar);
                // break;
                default:
                    throw new ArgumentException("Unsupported output mode.");
            }

            outputStopButton.Enabled = false;
            OutputLoop.IsForcedStopOutput = false;
        }

        private void OutputProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            outputProgressBar.Value = e.ProgressPercentage;
            outputProgressBar.Update();
        }
    }
}
