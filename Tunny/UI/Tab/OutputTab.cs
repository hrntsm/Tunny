using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Handler;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        private void OutputParatoSolutionButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            RunOutputLoop(OutputMode.ParatoSolutions);
        }

        private void OutputAllTrialsButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            RunOutputLoop(OutputMode.AllTrials);
        }

        private void OutputTrialNumberButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            RunOutputLoop(OutputMode.TrialNumber);
        }

        private void ReflectToSliderButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            RunOutputLoop(OutputMode.ReflectToSliders);
        }

        private void RunOutputLoop(OutputMode mode)
        {
            TLog.MethodStart();
            outputStopButton.Enabled = true;
            OutputLoop.Mode = mode;
            OutputLoop.Settings = _settings;
            OutputLoop.StudyName = outputTargetStudyComboBox.Text;
            if (string.IsNullOrEmpty(OutputLoop.StudyName))
            {
                WPF.Common.TunnyMessageBox.Show(
                    "Please select study name.",
                    "Tunny",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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
            TLog.MethodStart();
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
                case OutputMode.TrialNumber:
                    result = ParseTrialNumberInput(ref indices);
                    if (result)
                    {
                        OutputLoop.Indices = indices.ToArray();
                    }
                    break;
                case OutputMode.ReflectToSliders:
                    result = ParseTrialNumberInput(ref indices);
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

        private bool ParseTrialNumberInput(ref List<int> indices)
        {
            TLog.MethodStart();
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
            TLog.MethodStart();
            if (indices.Length > 1)
            {
                UseFirstTrialNumberToReflectMessage();
            }
        }

        private void OutputStopButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            outputResultBackgroundWorker?.Dispose();
            OutputLoop.IsForcedStopOutput = true;
            switch (OutputLoop.Mode)
            {
                case OutputMode.ParatoSolutions:
                case OutputMode.AllTrials:
                case OutputMode.TrialNumber:
                    _component.ExpireSolution(true);
                    break;
                case OutputMode.ReflectToSliders:
                    _component.GhInOut.NewSolution(_component.Fishes[0].GetParameterClassFormatVariables());
                    break;
                default:
                    throw new ArgumentException("Unsupported output mode.");
            }

            outputStopButton.Enabled = false;
            OutputLoop.IsForcedStopOutput = false;
        }

        private void OutputProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            TLog.MethodStart();
            outputProgressBar.Value = e.ProgressPercentage;
            outputProgressBar.Update();
        }
    }
}
