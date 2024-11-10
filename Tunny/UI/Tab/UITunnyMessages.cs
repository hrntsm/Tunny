using System.Windows;

using Grasshopper.GUI;

using Tunny.Core.Util;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        // OptimizationTab ==========================
        private bool SupportOneObjectiveMessage(GH_DocumentEditor ghCanvas)
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "CMA-ES & GP:Optuna samplers only support single objective optimization.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            ghCanvas.EnableUI();
            optimizeRunButton.Enabled = true;
            return false;
        }

        private bool SameStudyNameMassage(GH_DocumentEditor ghCanvas)
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "Please choose any study name.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            ghCanvas.EnableUI();
            optimizeRunButton.Enabled = true;
            return false;
        }

        private bool NameAlreadyExistMessage(GH_DocumentEditor ghCanvas)
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "New study name already exists. Please choose another name. Or check 'Continue' checkbox.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            ghCanvas.EnableUI();
            optimizeRunButton.Enabled = true;
            return false;
        }

        // OutputTab ==========================
        private static bool IncorrectParseModeNumberInputMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "The model number format of the input is incorrect. \nPlease use a comma separator as follows.\n\"1,2,3\"",
                "Tunny");
            return false;
        }

        private static void UseFirstTrialNumberToReflectMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "You input multi trial numbers, but this function only reflect variables to slider or gene pool to first one.",
                "Tunny"
            );
        }

        // VisualizeTab ==========================
        private static bool HandleOnly1ObjectiveMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "This plot can only handle one objective function.",
                "Tunny"
            );
            return false;
        }

        private static bool HandleOnly2or3ObjectiveMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "This plot can only handle 2 or 3 objective function.",
                "Tunny"
            );
            return false;
        }

        private static bool RequireLeast1VariableMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "This plot requires at least one variable.",
                "Tunny"
            );
            return false;
        }

        private static bool RequireLeast2VariableMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "This plot requires at least two variables.",
                "Tunny"
            );
            return false;
        }
    }
}
