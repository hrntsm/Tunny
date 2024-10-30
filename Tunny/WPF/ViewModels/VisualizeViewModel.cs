using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using CefSharp;
using CefSharp.Wpf;

using Optuna.Study;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.Core.Storage;
using Tunny.Process;
using Tunny.Solver;
using Tunny.WPF.Common;
using Tunny.WPF.Models;
using Tunny.WPF.Views.Pages.Visualize;

namespace Tunny.WPF.ViewModels
{
    internal class VisualizeViewModel : BindableBase
    {
        private readonly TSettings _settings;
        private VisualizeType _targetVisualizeType;


        private ChromiumWebBrowser _plotFrame;
        public ChromiumWebBrowser PlotFrame { get => _plotFrame; set => SetProperty(ref _plotFrame, value); }

        private ObservableCollection<VisualizeListItem> _objectiveItems;
        public ObservableCollection<VisualizeListItem> ObjectiveItems { get => _objectiveItems; set => SetProperty(ref _objectiveItems, value); }
        private ObservableCollection<VisualizeListItem> _variableItems;
        public ObservableCollection<VisualizeListItem> VariableItems { get => _variableItems; set => SetProperty(ref _variableItems, value); }

        private Page _plotSettingsFrame;
        public Page PlotSettingsFrame { get => _plotSettingsFrame; set => SetProperty(ref _plotSettingsFrame, value); }

        public VisualizeViewModel()
        {
            _settings = OptimizeProcess.Settings;
            PlotFrame = new ChromiumWebBrowser();
            PlotSettingsFrame = new ParetoFrontPage();
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

        private void UpdateListView()
        {
            StudySummary[] summaries = new StorageHandler().GetStudySummaries(_settings.Storage.Path);
            //StudySummary targetStudySummary = summaries.FirstOrDefault(s => s.StudyName == _selectedStudyName.Name);
            //if (targetStudySummary != null)
            //{
            //    SetVariableListItems(targetStudySummary);
            //    PlotSettingsFrame.SetTargetStudy(targetStudySummary);
            //}
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

        internal void SetTargetVisualizeType(VisualizeType visualizeType)
        {
            _targetVisualizeType = visualizeType;
            SetSettingsPage();
            string resource = "Tunny.WPF.Assets.html.visualize_top.html";
            string content = LoadEmbeddedHtml(resource);
            PlotFrame.LoadHtml(content);
        }

        private void SetSettingsPage()
        {
            switch (_targetVisualizeType)
            {
                case VisualizeType.ParetoFront:
                    PlotSettingsFrame = new ParetoFrontPage();
                    break;
                case VisualizeType.OptimizationHistory:
                    PlotSettingsFrame = new OptimizationHistoryPage();
                    break;
                case VisualizeType.Slice:
                    break;
                case VisualizeType.Contour:
                    break;
                case VisualizeType.ParameterImportance:
                    break;
                case VisualizeType.ParallelCoordinate:
                    break;
                case VisualizeType.Hypervolume:
                    break;
                case VisualizeType.EDF:
                    break;
                case VisualizeType.Rank:
                    break;
                case VisualizeType.TimeLine:
                    break;
                case VisualizeType.TerminatorImprovement:
                    break;
                case VisualizeType.OptunaHub:
                    break;
            }
        }

        private static string LoadEmbeddedHtml(string resource)
        {
            string content = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    content = new StreamReader(stream).ReadToEnd();
                }
            }

            return content;
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

        private async void Plot()
        {
            SetLoadingPage();
            switch (_targetVisualizeType)
            {
                case VisualizeType.ParetoFront:
                    await Task.Run(() => PlotParetoFrontAsync());
                    break;
                case VisualizeType.OptimizationHistory:
                    await Task.Run(() => PlotOptimizationHistoryAsync());
                    break;
                case VisualizeType.Slice:
                    await Task.Run(() => PlotSliceAsync());
                    break;
                case VisualizeType.Contour:
                    await Task.Run(() => PlotContourAsync());
                    break;
                case VisualizeType.ParameterImportance:
                    await Task.Run(() => PlotParameterImportanceAsync());
                    break;
                case VisualizeType.Hypervolume:
                    await Task.Run(() => PlotHypervolumeAsync());
                    break;
                case VisualizeType.EDF:
                    await Task.Run(() => PlotEdfAsync());
                    break;
                case VisualizeType.Rank:
                    await Task.Run(() => PlotRankAsync());
                    break;
                case VisualizeType.TimeLine:
                    await Task.Run(() => PlotTimeLineAsync());
                    break;
                case VisualizeType.TerminatorImprovement:
                    await Task.Run(() => PlotTerminatorImprovementAsync());
                    break;
                case VisualizeType.OptunaHub:
                    break;
            }
        }

        private void SetLoadingPage()
        {
            string resource = "Tunny.WPF.Assets.html.loading.html";
            string content = LoadEmbeddedHtml(resource);
            PlotFrame.LoadHtml(content);
        }

        private void PlotParetoFrontAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            var viewModel = (IPlotSettings)PlotSettingsFrame.DataContext;
            Plot settings = viewModel.GetPlotSettings();
            //settings.TargetStudyName = SelectedStudyName.Name;
            string htmlPath = vis.Plot(settings);
            string fileUrl = "file:///" + htmlPath.Replace("\\", "/");
            PlotFrame.Load(fileUrl);
        }

        private void PlotOptimizationHistoryAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotSliceAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotContourAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotParameterImportanceAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotHypervolumeAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotEdfAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotRankAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotTimeLineAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }

        private void PlotTerminatorImprovementAsync()
        {
            var vis = new Visualize(_settings, OptimizeProcess.Component.GhInOut.HasConstraint);
            TunnyMessageBox.Error_NoImplemented();
        }
    }
}
