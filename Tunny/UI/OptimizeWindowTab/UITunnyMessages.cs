using System.Windows.Forms;

using Grasshopper.GUI;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        // OptimizationTab ==========================
        private bool CmaEsSupportOneObjectiveMessage(GH_DocumentEditor ghCanvas)
        {
            TunnyMessageBox.Show(
                "CMA-ES samplers only support single objective optimization.",
                "Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            ghCanvas.EnableUI();
            optimizeRunButton.Enabled = true;
            return false;
        }

        private bool SameStudyNameMassage(GH_DocumentEditor ghCanvas)
        {
            TunnyMessageBox.Show(
                "Please choose any study name.",
                "Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            ghCanvas.EnableUI();
            optimizeRunButton.Enabled = true;
            return false;
        }

        private bool NameAlreadyExistMessage(GH_DocumentEditor ghCanvas)
        {
            TunnyMessageBox.Show(
                "New study name already exists. Please choose another name. Or check 'Continue' checkbox.",
                "Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            ghCanvas.EnableUI();
            optimizeRunButton.Enabled = true;
            return false;
        }

        // OutputTab ==========================
        private static bool IncorrectParseModeNumberInputMessage()
        {
            TunnyMessageBox.Show(
                "The model number format of the input is incorrect. \nPlease use a comma separator as follows.\n\"1,2,3\"",
                "Tunny");
            return false;
        }

        private static void UseFirstModelNumberToReflectMessage()
        {
            TunnyMessageBox.Show(
                "You input multi model numbers, but this function only reflect variables to slider or gene pool to first one.",
                "Tunny"
            );
        }

        // VisualizeTab ==========================
        private static void ResultFileNotExistErrorMessage()
        {
            TunnyMessageBox.Show(
                "Please set exist result file path.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private static bool HandleOnly1ObjectiveMessage()
        {
            TunnyMessageBox.Show(
                "This plot can only handle one objective function.",
                "Tunny"
            );
            return false;
        }

        private static bool HandleOnly2ObjectivesMessage()
        {
            TunnyMessageBox.Show(
                "This plot can only handle 2 objective functions.",
                "Tunny"
            );
            return false;
        }

        private static bool HandleOnly2or3ObjectiveMessage()
        {
            TunnyMessageBox.Show(
                "This plot can only handle 2 or 3 objective function.",
                "Tunny"
            );
            return false;
        }

        private static bool RequireLeast1VariableMessage()
        {
            TunnyMessageBox.Show(
                "This plot requires at least one variable.",
                "Tunny"
            );
            return false;
        }

        private static bool RequireLeast2VariableMessage()
        {
            TunnyMessageBox.Show(
                "This plot requires at least two variables.",
                "Tunny"
            );
            return false;
        }
    }
}
