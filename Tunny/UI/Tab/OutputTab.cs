using System;
using System.ComponentModel;

using Tunny.Core.TEnum;
using Tunny.Core.Util;

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
        }

        private void OutputStopButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
        }

        private void OutputProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            TLog.MethodStart();
            outputProgressBar.Value = e.ProgressPercentage;
            outputProgressBar.Update();
        }
    }
}
