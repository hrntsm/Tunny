using System;
using System.Collections.ObjectModel;
using System.Linq;

using Optuna.Study;

using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.WPF.Common;
using Tunny.WPF.Models;

namespace Tunny.WPF.ViewModels
{
    internal class ParetoFrontViewModel : BindableBase, IPlotSettings
    {
        private ObservableCollection<NameComboBoxItem> _studyNameItems;
        public ObservableCollection<NameComboBoxItem> StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private NameComboBoxItem _selectedStudyName;
        public NameComboBoxItem SelectedStudyName { get => _selectedStudyName; set => SetProperty(ref _selectedStudyName, value); }
        private ObservableCollection<VisualizeListItem> _objectiveItems;
        public ObservableCollection<VisualizeListItem> ObjectiveItems { get => _objectiveItems; set => SetProperty(ref _objectiveItems, value); }

        private bool? _includeDominatedTrials;
        public bool? IncludeDominatedTrials { get => _includeDominatedTrials; set => SetProperty(ref _includeDominatedTrials, value); }

        public ParetoFrontViewModel()
        {
            ObjectiveItems = new ObservableCollection<VisualizeListItem>();
            IncludeDominatedTrials = true;
        }

        public void SetTargetStudy(StudySummary targetStudySummary)
        {
            if (targetStudySummary != null)
            {
                SetObjectiveListItems(targetStudySummary);
            }
        }

        private void SetObjectiveListItems(StudySummary visualizeStudySummary)
        {
            string versionString = (visualizeStudySummary.UserAttrs["tunny_version"] as string[])[0];
            var version = new Version(versionString);
            string[] metricNames = version <= TEnvVariables.OldStorageVersion
                ? visualizeStudySummary.UserAttrs["objective_names"] as string[]
                : visualizeStudySummary.SystemAttrs["study:metric_names"] as string[];
            ObjectiveItems.Clear();
            for (int i = 0; i < metricNames.Length; i++)
            {
                ObjectiveItems.Add(new VisualizeListItem()
                {
                    Name = metricNames[i]
                });
            }
        }

        public Plot GetPlotSettings()
        {
            return new Plot()
            {
                PlotActionType = PlotActionType.Show,
                PlotTypeName = "pareto front",
                IncludeDominatedTrials = IncludeDominatedTrials.Value,
                TargetObjectiveName = ObjectiveItems.Where(o => o.IsSelected).Select(o => o.Name).ToArray(),
                TargetObjectiveIndex = ObjectiveItems.Where(o => o.IsSelected).Select(o => ObjectiveItems.IndexOf(o)).ToArray()
            };
        }
    }
}
