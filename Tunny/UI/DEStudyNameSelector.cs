using System;
using System.Linq;
using System.Windows.Forms;

using Optuna.Study;

using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.Storage;
using Tunny.Core.Util;

namespace Tunny.UI
{
    public partial class DEStudyNameSelector : Form
    {
        private readonly Storage _storage;

        public DEStudyNameSelector(Storage storage)
        {
            TLog.MethodStart();
            TLog.Info("DEStudyNameSelector is open");

            _storage = storage;
            InitializeComponent();
            UpdateStudyNameComboBox();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Window_Load(object sender, EventArgs e)
        {
            TLog.MethodStart();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void FormClosingXButton(object sender, FormClosingEventArgs e)
        {
            TLog.MethodStart();
            TLog.Info("DEStudyNameSelector is closed");
        }

        private void UpdateStudyNameComboBox()
        {
            TLog.MethodStart();
            studyNameComboBox.Items.Clear();
            string storagePath = _storage.Path;

            TLog.Info($"Get study summaries from storage file: {storagePath}");
            StudySummary[] summaries = new StorageHandler().GetStudySummaries(storagePath);

            if (summaries.Length > 0)
            {
                string[] studyNames = summaries.Select(summary => summary.StudyName).ToArray();
                studyNameComboBox.Items.AddRange(studyNames);
                studyNameComboBox.SelectedIndex = 0;
            }
            else
            {
                TLog.Warning("No study summaries found");
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            string selectedStudyName = studyNameComboBox.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedStudyName))
            {
                TLog.Warning("No study name selected");
                return;
            }
            TLog.Info($"Selected study name: {selectedStudyName}");

            var designExplorer = new DesignExplorer(selectedStudyName, _storage);
            designExplorer.Run();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            TLog.Info("DEStudyNameSelector is closed");
            Close();
        }
    }
}
