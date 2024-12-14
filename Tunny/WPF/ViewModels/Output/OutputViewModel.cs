using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;

using Optuna.Study;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.Core.Util;
using Tunny.WPF.Common;
using Tunny.WPF.Models;
using Tunny.WPF.Views.Pages.Optimize;

namespace Tunny.WPF.ViewModels.Output
{
    internal sealed class OutputViewModel : BindableBase
    {
        public OutputViewModel()
        {
            StudySummary[] summaries = SharedItems.Instance.StudySummaries;
            StudyNameItems = Utils.StudyNamesFromStudySummaries(summaries);
            SelectedStudyName = StudyNameItems.FirstOrDefault();
            OutputTargetItems = new ObservableCollection<OutputTrialItem>();

            Dictionary<int, ObservableCollection<OutputTrialItem>> dict = SharedItems.Instance.OutputTrialDict;
            if (!dict.TryGetValue(SelectedStudyName.Id, out ObservableCollection<OutputTrialItem> item))
            {
                item = new ObservableCollection<OutputTrialItem>();
                dict.Add(SelectedStudyName.Id, item);
            }
            OutputListedItems = item;
            InitializeChart();
        }

        private void InitializeChart()
        {
            _chart1 = new LiveChartPage();
            OutputChart1 = _chart1;
            _chart2 = new LiveChartPage();
            OutputChart2 = _chart2;
        }

        private LiveChartPage _chart1;
        private LiveChartPage _chart2;

        private ObservableCollection<NameComboBoxItem> _studyNameItems;
        public ObservableCollection<NameComboBoxItem> StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private NameComboBoxItem _selectedStudyName;
        public NameComboBoxItem SelectedStudyName
        {
            get => _selectedStudyName;
            set
            {
                SetProperty(ref _selectedStudyName, value);
                if (!SharedItems.Instance.OutputTrialDict.TryGetValue(value.Id, out ObservableCollection<OutputTrialItem> items))
                {
                    items = new ObservableCollection<OutputTrialItem>();
                    SharedItems.Instance.OutputTrialDict.Add(value.Id, items);
                }
                if (OutputListedItems != null)
                {
                    OutputTargetItems.Clear();
                }
                OutputListedItems = items;
            }
        }
        private string _targetTrialNumber;
        public string TargetTrialNumber { get => _targetTrialNumber; set => SetProperty(ref _targetTrialNumber, value); }

        private ObservableCollection<OutputTrialItem> _outputListedItems;
        public ObservableCollection<OutputTrialItem> OutputListedItems { get => _outputListedItems; set => SetProperty(ref _outputListedItems, value); }
        private ObservableCollection<OutputTrialItem> _outputTargetItems;
        public ObservableCollection<OutputTrialItem> OutputTargetItems { get => _outputTargetItems; set => SetProperty(ref _outputTargetItems, value); }
        private object _outputChart1;
        public object OutputChart1 { get => _outputChart1; set => SetProperty(ref _outputChart1, value); }
        private object _outputChart2;
        public object OutputChart2 { get => _outputChart2; set => SetProperty(ref _outputChart2, value); }

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
            if (string.IsNullOrEmpty(TargetTrialNumber))
            {
                return;
            }
            Dictionary<int, ObservableCollection<OutputTrialItem>> dict = SharedItems.Instance.OutputTrialDict;
            if (!dict.TryGetValue(SelectedStudyName.Id, out ObservableCollection<OutputTrialItem> item))
            {
                item = new ObservableCollection<OutputTrialItem>();
                dict.Add(SelectedStudyName.Id, item);
            }

            int trialId = int.Parse(TargetTrialNumber, NumberStyles.Integer, CultureInfo.InvariantCulture);
            if (item.Any(x => x.Id == trialId))
            {
                return;
            }
            OutputTrialItem outputTrialItem = SharedItems.Instance.GetOutputTrial(SelectedStudyName.Id, trialId);
            item.Add(outputTrialItem);
        }

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
            TLog.MethodStart();
            if (!File.Exists(SharedItems.Instance.Settings.Storage.Path))
            {
                TunnyMessageBox.Error_ResultFileNotExist();
                return;
            }
            string dashboardPath = Path.Combine(TEnvVariables.TunnyEnvPath, "python", "Scripts", "optuna-dashboard.exe");
            string storagePath = SharedItems.Instance.Settings.Storage.Path;

            var dashboard = new Optuna.Dashboard.Handler(dashboardPath, storagePath);
            string path = Path.Combine("dashboard", "studies", SelectedStudyName.Id.ToString(CultureInfo.InvariantCulture), "trialCompare");
            dashboard.Run(true, path);
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
                var removedList = OutputListedItems.Where(i => i.IsSelected == false).ToList();
                OutputListedItems = new ObservableCollection<OutputTrialItem>(removedList);
            }
            else if (target == "OutputTargetTrials")
            {
                var remainItems = new List<OutputTrialItem>();
                foreach (OutputTrialItem item in OutputTargetItems)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = false;
                        OutputListedItems.Add(item);
                        SharedItems.Instance.OutputTrialDict[SelectedStudyName.Id].Add(item);
                    }
                    else
                    {
                        remainItems.Add(item);
                    }
                }
                OutputTargetItems = new ObservableCollection<OutputTrialItem>(remainItems);
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
                OutputListedItems.Clear();
                SharedItems.Instance.OutputTrialDict[SelectedStudyName.Id].Clear();
            }
            else if (target == "OutputTargetTrials")
            {
                foreach (OutputTrialItem item in OutputTargetItems)
                {
                    item.IsSelected = false;
                    OutputListedItems.Add(item);
                    SharedItems.Instance.OutputTrialDict[SelectedStudyName.Id].Add(item);
                }
                OutputTargetItems.Clear();
            }
        }

        private DelegateCommand _addOutputTargetListCommand;
        public ICommand AddOutputTargetListCommand
        {
            get
            {
                if (_addOutputTargetListCommand == null)
                {
                    _addOutputTargetListCommand = new DelegateCommand(AddOutputTargetList);
                }
                return _addOutputTargetListCommand;
            }
        }
        private void AddOutputTargetList()
        {
            var selected = OutputListedItems.Where(x => x.IsSelected).ToList();
            foreach (OutputTrialItem item in selected)
            {
                OutputListedItems.Remove(item);
                OutputTargetItems.Add(item);
            }
        }

        private DelegateCommand _addAllCommand;
        public ICommand AddAllCommand
        {
            get
            {
                if (_addAllCommand == null)
                {
                    _addAllCommand = new DelegateCommand(AddAll);
                }
                return _addAllCommand;
            }
        }
        private void AddAll()
        {
            Dictionary<int, ObservableCollection<OutputTrialItem>> dict = SharedItems.Instance.OutputTrialDict;
            if (!dict.TryGetValue(SelectedStudyName.Id, out ObservableCollection<OutputTrialItem> item))
            {
                item = new ObservableCollection<OutputTrialItem>();
                dict.Add(SelectedStudyName.Id, item);
            }

            IEnumerable<int> trialIds = SharedItems.Instance.Trials[SelectedStudyName.Id].Select(x => x.TrialId);
            foreach (int trialId in trialIds)
            {
                if (item.Any(x => x.Id == trialId))
                {
                    continue;
                }
                OutputTrialItem outputTrialItem = SharedItems.Instance.GetOutputTrial(SelectedStudyName.Id, trialId);
                item.Add(outputTrialItem);
            }
        }

        private DelegateCommand _addParetoFrontCommand;
        public ICommand AddParetoFrontCommand
        {
            get
            {
                if (_addParetoFrontCommand == null)
                {
                    _addParetoFrontCommand = new DelegateCommand(AddParetoFront);
                }
                return _addParetoFrontCommand;
            }
        }
        private void AddParetoFront()
        {
        }

        private DelegateCommand _addFeasibleCommand;
        public ICommand AddFeasibleCommand
        {
            get
            {
                if (_addFeasibleCommand == null)
                {
                    _addFeasibleCommand = new DelegateCommand(AddFeasible);
                }
                return _addFeasibleCommand;
            }
        }
        private void AddFeasible()
        {
        }

        private DelegateCommand _openDesignExplorerSelectionCommand;
        public ICommand OpenDesignExplorerSelectionCommand
        {
            get
            {
                if (_openDesignExplorerSelectionCommand == null)
                {
                    _openDesignExplorerSelectionCommand = new DelegateCommand(OpenDesignExplorerSelection);
                }
                return _openDesignExplorerSelectionCommand;
            }
        }
        private void OpenDesignExplorerSelection()
        {
        }

        private DelegateCommand _loadDesignExplorerSelectionCommand;
        public ICommand LoadDesignExplorerSelectionCommand
        {
            get
            {
                if (_loadDesignExplorerSelectionCommand == null)
                {
                    _loadDesignExplorerSelectionCommand = new DelegateCommand(LoadDesignExplorerSelection);
                }
                return _loadDesignExplorerSelectionCommand;
            }
        }
        private void LoadDesignExplorerSelection()
        {
        }

        private DelegateCommand<string> _checkAllCommand;
        public ICommand CheckAllCommand
        {
            get
            {
                if (_checkAllCommand == null)
                {
                    _checkAllCommand = new DelegateCommand<string>(CheckAll);
                }
                return _checkAllCommand;
            }
        }
        private void CheckAll(string target)
        {
            if (target == "ListedTrials")
            {
                foreach (OutputTrialItem item in OutputListedItems)
                {
                    item.IsSelected = IsListedAllChecked ?? false;
                }
            }
            else if (target == "OutputTargetTrials")
            {
                foreach (OutputTrialItem item in OutputTargetItems)
                {
                    item.IsSelected = IsTargetsAllChecked ?? false;
                }
            }
        }

        private bool? _isTargetsAllChecked;
        public bool? IsTargetsAllChecked { get => _isTargetsAllChecked; set => SetProperty(ref _isTargetsAllChecked, value); }
        private bool? _isListedAllChecked;
        public bool? IsListedAllChecked { get => _isListedAllChecked; set => SetProperty(ref _isListedAllChecked, value); }
    }
}
