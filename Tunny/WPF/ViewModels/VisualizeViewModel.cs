﻿using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

using CefSharp;
using CefSharp.Wpf;

using Optuna.Study;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.Core.Storage;
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
                SetVariableListItems(targetStudySummary);
                PlotSettingsFrame.SetTargetStudy(targetStudySummary);
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

        public VisualizeViewModel()
        {
        }

        public VisualizeViewModel(TSettings settings)
        {
            _settings = settings;
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

        internal void SetTargetVisualizeType(VisualizeType visualizeType)
        {
            _targetVisualizeType = visualizeType;
            string resource = "Tunny.WPF.Assets.html.visualize_top.html";
            string content = LoadEmbeddedHtml(resource);
            PlotFrame.LoadHtml(content);
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
            SetLoadingPage();
            await Task.Run(() => PlotParetoFrontAsync());
        }

        private void SetLoadingPage()
        {
            string resource = "Tunny.WPF.Assets.html.loading.html";
            string content = LoadEmbeddedHtml(resource);
            PlotFrame.LoadHtml(content);
        }

        private void PlotParetoFrontAsync()
        {
            var vis = new Visualize(_settings, false);
            Plot settings = PlotSettingsFrame.GetPlotSettings();
            settings.TargetStudyName = SelectedStudyName.Name;
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

        private ParetoFrontPage _plotSettingsFrame;
        public ParetoFrontPage PlotSettingsFrame { get => _plotSettingsFrame; set => SetProperty(ref _plotSettingsFrame, value); }
    }
}
