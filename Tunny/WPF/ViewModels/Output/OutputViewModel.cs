using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

using Optuna.Study;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.WPF.Common;
using Tunny.WPF.Models;

namespace Tunny.WPF.ViewModels.Output
{
    internal sealed class OutputViewModel : BindableBase
    {
        public OutputViewModel()
        {
            StudySummary[] summaries = SharedItems.Instance.StudySummaries;
            StudyNameItems = Utils.StudyNamesFromStudySummaries(summaries);
            SelectedStudyName = StudyNameItems.FirstOrDefault();
            OutputListedItems = new ObservableCollection<OutputTrialItem>();
            OutputTargetItems = new ObservableCollection<OutputTrialItem>();

            Dictionary<int, List<OutputTrialItem>> dict = SharedItems.Instance.OutputTrialDict;
            if (!dict.TryGetValue(SelectedStudyName.Id, out List<OutputTrialItem> item))
            {
                item = new List<OutputTrialItem>();
                dict.Add(SelectedStudyName.Id, item);
            }
            OutputListedItems = new ObservableCollection<OutputTrialItem>(item);
        }

        private ObservableCollection<NameComboBoxItem> _studyNameItems;
        public ObservableCollection<NameComboBoxItem> StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private NameComboBoxItem _selectedStudyName;
        public NameComboBoxItem SelectedStudyName { get => _selectedStudyName; set => SetProperty(ref _selectedStudyName, value); }
        private string _targetTrialNumber;
        public string TargetTrialNumber { get => _targetTrialNumber; set => SetProperty(ref _targetTrialNumber, value); }

        private object _outputChart1;
        public object OutputChart1 { get => _outputChart1; set => SetProperty(ref _outputChart1, value); }
        private object _outputChart2;
        public object OutputChart2 { get => _outputChart2; set => SetProperty(ref _outputChart2, value); }
        private ObservableCollection<OutputTrialItem> _outputListedItems;
        public ObservableCollection<OutputTrialItem> OutputListedItems { get => _outputListedItems; set => SetProperty(ref _outputListedItems, value); }

        private DelegateCommand _addToListCommand;
        public ICommand AddToListCommand
        {
            get
            {
                if (_addToListCommand == null)
                {
                    _addToListCommand = new DelegateCommand(AddToList);
                }
                return _addToListCommand;
            }
        }
        private void AddToList()
        {
            Dictionary<int, List<OutputTrialItem>> dict = SharedItems.Instance.OutputTrialDict;
            if (!dict.TryGetValue(SelectedStudyName.Id, out List<OutputTrialItem> item))
            {
                item = new List<OutputTrialItem>();
                dict.Add(SelectedStudyName.Id, item);
            }
            var outputTrialItem = new OutputTrialItem()
            {
                IsSelected = false,
                Id = int.Parse(TargetTrialNumber, NumberStyles.Integer, CultureInfo.InvariantCulture),
            };
            if (item.Any(x => x.Id == outputTrialItem.Id))
            {
                return;
            }
            item.Add(outputTrialItem);
            OutputListedItems.Add(outputTrialItem);
        }

        private ObservableCollection<OutputTrialItem> _outputTargetItems;
        public ObservableCollection<OutputTrialItem> OutputTargetItems { get => _outputTargetItems; set => SetProperty(ref _outputTargetItems, value); }

        private DelegateCommand _loadDashboardSelectionCommand;
        public ICommand LoadDashboardSelectionCommand
        {
            get
            {
                if (_loadDashboardSelectionCommand == null)
                {
                    _loadDashboardSelectionCommand = new DelegateCommand(LoadDashboardSelection);
                }
                return _loadDashboardSelectionCommand;
            }
        }
        private void LoadDashboardSelection()
        {
        }

        private DelegateCommand _openOptunaDashboardTrialSelectionCommand;

        public ICommand OpenOptunaDashboardTrialSelectionCommand
        {
            get
            {
                if (_openOptunaDashboardTrialSelectionCommand == null)
                {
                    _openOptunaDashboardTrialSelectionCommand = new DelegateCommand(OpenOptunaDashboardTrialSelection);
                }
                return _openOptunaDashboardTrialSelectionCommand;
            }
        }
        private void OpenOptunaDashboardTrialSelection()
        {
        }

        private DelegateCommand _reinstateCommand;
        public ICommand ReinstateCommand
        {
            get
            {
                if (_reinstateCommand == null)
                {
                    _reinstateCommand = new DelegateCommand(Reinstate);
                }
                return _reinstateCommand;
            }
        }
        private void Reinstate()
        {
        }

        private DelegateCommand<string> _removeFromListCommand;
        public ICommand RemoveFromListCommand
        {
            get
            {
                if (_removeFromListCommand == null)
                {
                    _removeFromListCommand = new DelegateCommand<string>(RemoveFromList);
                }
                return _removeFromListCommand;
            }
        }
        private void RemoveFromList(string target)
        {
            if (target == "ListedTrials")
            {
            }
            else if (target == "OutputTargetTrials")
            {
            }
        }

        private DelegateCommand<string> _clearListCommand;
        public ICommand ClearListCommand
        {
            get
            {
                if (_clearListCommand == null)
                {
                    _clearListCommand = new DelegateCommand<string>(ClearList);
                }
                return _clearListCommand;
            }
        }
        private void ClearList(string target)
        {
            if (target == "ListedTrials")
            {
            }
            else if (target == "OutputTargetTrials")
            {
            }
        }
    }
}
