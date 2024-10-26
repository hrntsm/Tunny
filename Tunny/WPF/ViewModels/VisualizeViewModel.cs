using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using CefSharp;
using CefSharp.Wpf;

using Optuna.Study;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.Core.Storage;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Solver;
using Tunny.WPF.Common;
using Tunny.WPF.Models;

namespace Tunny.WPF.ViewModels
{
    internal class VisualizeViewModel : BindableBase
    {
        private readonly TSettings _settings;
        private VisualizeType _targetVisualizeType;

        private ObservableCollection<NameComboBoxItem> _studyNameItems;
        public ObservableCollection<NameComboBoxItem> StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private NameComboBoxItem _selectedStudyName;
        public NameComboBoxItem SelectedStudyName
        {
            get => _selectedStudyName;
            set
            {
                SetProperty(ref _selectedStudyName, value);
                UpdateListView();
            }
        }

        private void UpdateListView()
        {
            StudySummary[] summaries = new StorageHandler().GetStudySummaries(_settings.Storage.Path);
            StudySummary targetStudySummary = summaries.FirstOrDefault(s => s.StudyName == _selectedStudyName.Name);
            if (targetStudySummary != null)
            {
                SetObjectiveListItems(targetStudySummary);
                SetVariableListItems(targetStudySummary);
            }
        }

        private void SetVariableListItems(StudySummary visualizeStudySummary)
        {
            string[] variableNames = visualizeStudySummary.UserAttrs["variable_names"] as string[];
            VariableItems.Clear();
            for (int i = 0; i < variableNames.Length; i++)
            {
                VariableItems.Add(new VisualizeListItem()
                {
                    Name = variableNames[i]
                });
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

        public VisualizeViewModel()
        {
        }

        public VisualizeViewModel(TSettings settings)
        {
            _settings = settings;
            PlotFrame = new ChromiumWebBrowser();
            ObjectiveItems = new ObservableCollection<VisualizeListItem>();
            VariableItems = new ObservableCollection<VisualizeListItem>();
        }

        private static ObservableCollection<NameComboBoxItem> StudyNamesFromStorage(string storagePath)
        {
            var items = new ObservableCollection<NameComboBoxItem>();
            StudySummary[] summaries = new StorageHandler().GetStudySummaries(storagePath);
            for (int i = 0; i < summaries.Length; i++)
            {
                StudySummary summary = summaries[i];
                items.Add(new NameComboBoxItem()
                {
                    Id = i,
                    Name = summary.StudyName
                });
            }
            return items;
        }

        internal void SetTargetVisualizeType(VisualizeType visualizeType)
        {
            _targetVisualizeType = visualizeType;
            PlotFrame.LoadHtml("<html><body><h3>Select your target and press the Plot button</h3></body></html>");
        }

        internal void SetStudyNameItems(TSettings settings)
        {
            StudyNameItems = StudyNamesFromStorage(settings.Storage.Path);
            SelectedStudyName = StudyNameItems[0];
        }

        private DelegateCommand _plotCommand;

        public ICommand PlotCommand
        {
            get
            {
                if (_plotCommand == null)
                {
                    _plotCommand = new DelegateCommand(Plot);
                }

                return _plotCommand;
            }
        }

        private void Plot()
        {
            switch (_targetVisualizeType)
            {
                case VisualizeType.ParetoFront:
                    PlotParetoFront();
                    break;
                case VisualizeType.OptimizationHistory:
                    break;
                case VisualizeType.Slice:
                    break;
                case VisualizeType.Contour:
                    break;
                case VisualizeType.ParameterImportance:
                    break;
                case VisualizeType.Hypervolume:
                    break;
                case VisualizeType.EDF:
                    break;
                case VisualizeType.OptunaHub:
                    break;
            }
        }

        private async void PlotParetoFront()
        {
            string loadingAnimationPath = "C:\\Users\\hiroa\\Desktop\\simple.html";
            string loadingUrl = "file:///" + loadingAnimationPath.Replace("\\", "/");
            PlotFrame.Load(loadingUrl);

            await Task.Run(() => PlotParetoFrontAsync());
        }

        private void PlotParetoFrontAsync()
        {
            var vis = new Visualize(_settings, false);
            var settings = new Plot
            {
                TargetStudyName = _selectedStudyName.Name,
                PlotActionType = PlotActionType.Show,
                PlotTypeName = "pareto front",
                TargetObjectiveName = ObjectiveItems.Where(o => o.IsSelected).Select(o => o.Name).ToArray(),
                TargetObjectiveIndex = ObjectiveItems.Where(o => o.IsSelected).Select(o => ObjectiveItems.IndexOf(o)).ToArray(),
                TargetVariableName = VariableItems.Where(v => v.IsSelected).Select(v => v.Name).ToArray(),
                TargetVariableIndex = VariableItems.Where(v => v.IsSelected).Select(v => VariableItems.IndexOf(v)).ToArray(),
                ClusterCount = 0,
                IncludeDominatedTrials = true
            };
            string htmlPath = vis.Plot(settings);
            string fileUrl = "file:///" + htmlPath.Replace("\\", "/");
            PlotFrame.Load(fileUrl);
        }

        private ChromiumWebBrowser _plotFrame;
        public ChromiumWebBrowser PlotFrame { get => _plotFrame; set => SetProperty(ref _plotFrame, value); }

        private ObservableCollection<VisualizeListItem> _objectiveItems;
        public ObservableCollection<VisualizeListItem> ObjectiveItems { get => _objectiveItems; set => SetProperty(ref _objectiveItems, value); }
        private ObservableCollection<VisualizeListItem> _variableItems;
        public ObservableCollection<VisualizeListItem> VariableItems { get => _variableItems; set => SetProperty(ref _variableItems, value); }
    }
}
