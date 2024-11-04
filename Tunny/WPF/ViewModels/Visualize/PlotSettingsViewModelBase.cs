using System;
using System.Collections.ObjectModel;
using System.Linq;

using Optuna.Study;

using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.Core.Storage;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Models;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal abstract class PlotSettingsViewModelBase : BindableBase, IPlotSettings
    {
        public abstract PlotSettings GetPlotSettings();

        private StudySummary[] _summaries;
        private ObservableCollection<NameComboBoxItem> _studyNameItems;
        public ObservableCollection<NameComboBoxItem> StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private NameComboBoxItem _selectedStudyName;
        public NameComboBoxItem SelectedStudyName
        {
            get => _selectedStudyName;
            set
            {
                SetProperty(ref _selectedStudyName, value);
                UpdateObjectivesAndVariables();
            }
        }
        private ObservableCollection<VisualizeListItem> _objectiveItems;
        public ObservableCollection<VisualizeListItem> ObjectiveItems { get => _objectiveItems; set => SetProperty(ref _objectiveItems, value); }
        private VisualizeListItem _selectedObjective;
        public VisualizeListItem SelectedObjective { get => _selectedObjective; set => SetProperty(ref _selectedObjective, value); }
        private ObservableCollection<VisualizeListItem> _variableItems;
        public ObservableCollection<VisualizeListItem> VariableItems { get => _variableItems; set => SetProperty(ref _variableItems, value); }
        private VisualizeListItem _selectedVariable;
        public VisualizeListItem SelectedVariable { get => _selectedVariable; set => SetProperty(ref _selectedVariable, value); }

        public PlotSettingsViewModelBase()
        {
            ObjectiveItems = new ObservableCollection<VisualizeListItem>();
            VariableItems = new ObservableCollection<VisualizeListItem>();
            StudyNameItems = StudyNamesFromStorage(OptimizeProcess.Settings.Storage.Path);
            SelectedStudyName = StudyNameItems.FirstOrDefault();
        }

        public void UpdateItems()
        {
            StudyNameItems = StudyNamesFromStorage(OptimizeProcess.Settings.Storage.Path);
            SelectedStudyName = StudyNameItems.FirstOrDefault();
        }

        private ObservableCollection<NameComboBoxItem> StudyNamesFromStorage(string storagePath)
        {
            var items = new ObservableCollection<NameComboBoxItem>();
            _summaries = new StorageHandler().GetStudySummaries(storagePath);
            for (int i = 0; i < _summaries.Length; i++)
            {
                StudySummary summary = _summaries[i];
                items.Add(new NameComboBoxItem()
                {
                    Id = i,
                    Name = summary.StudyName
                });
            }
            return items;
        }

        private void UpdateObjectivesAndVariables()
        {
            StudySummary targetStudySummary = _summaries.FirstOrDefault(s => s.StudyName == _selectedStudyName.Name);
            if (targetStudySummary != null)
            {
                SetVariableListItems(targetStudySummary);
                SetObjectiveListItems(targetStudySummary);
            }
        }

        private void SetObjectiveListItems(StudySummary visualizeStudySummary)
        {
            ObjectiveItems.Clear();

            string versionString = (visualizeStudySummary.UserAttrs["tunny_version"] as string[])[0];
            var version = new Version(versionString);
            string[] metricNames = version <= TEnvVariables.OldStorageVersion
                ? visualizeStudySummary.UserAttrs["objective_names"] as string[]
                : visualizeStudySummary.SystemAttrs["study:metric_names"] as string[];
            for (int i = 0; i < metricNames.Length; i++)
            {
                ObjectiveItems.Add(new VisualizeListItem()
                {
                    Name = metricNames[i]
                });
            }
            SelectedObjective = ObjectiveItems.FirstOrDefault();
        }

        private void SetVariableListItems(StudySummary visualizeStudySummary)
        {
            VariableItems.Clear();

            string[] variableNames = visualizeStudySummary.UserAttrs["variable_names"] as string[];
            for (int i = 0; i < variableNames.Length; i++)
            {
                VariableItems.Add(new VisualizeListItem()
                {
                    Name = variableNames[i]
                });
            }
            SelectedVariable = VariableItems.FirstOrDefault();
        }

    }
}
