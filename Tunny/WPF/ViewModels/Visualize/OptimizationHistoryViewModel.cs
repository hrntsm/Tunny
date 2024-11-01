﻿using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal class OptimizationHistoryViewModel : BindableBase, IPlotSettings
    {
        private System.Collections.IEnumerable _studyNameItems;
        public System.Collections.IEnumerable StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private object _selectedStudyName;
        public object SelectedStudyName { get => _selectedStudyName; set => SetProperty(ref _selectedStudyName, value); }
        private System.Collections.IEnumerable _objectiveItems;
        public System.Collections.IEnumerable ObjectiveItems { get => _objectiveItems; set => SetProperty(ref _objectiveItems, value); }
        private object _selectedObjective;
        public object SelectedObjective { get => _selectedObjective; set => SetProperty(ref _selectedObjective, value); }
        private System.Collections.IEnumerable _compareStudyNameItems;
        public System.Collections.IEnumerable CompareStudyNameItems { get => _compareStudyNameItems; set => SetProperty(ref _compareStudyNameItems, value); }
        private bool? _showErrorBar;
        public bool? ShowErrorBar { get => _showErrorBar; set => SetProperty(ref _showErrorBar, value); }

        public OptimizationHistoryViewModel()
        {
            ShowErrorBar = false;
        }

        public Plot GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
