using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using CefSharp;
using CefSharp.Wpf;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.Core.Storage;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages.Visualize;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal class VisualizeViewModel : BindableBase
    {
        private readonly TSettings _settings;
        private VisualizeType _targetVisualizeType;
        private Dictionary<VisualizeType, Lazy<Page>> _plotSettingPages;
        private Lazy<ChromiumWebBrowser> _plotFrame;
        public ChromiumWebBrowser PlotFrame { get => _plotFrame.Value; set => SetProperty(ref _plotFrame, new Lazy<ChromiumWebBrowser>(() => value)); }
        private Page _plotSettingsFrame;
        public Page PlotSettingsFrame { get => _plotSettingsFrame; set => SetProperty(ref _plotSettingsFrame, value); }

        public VisualizeViewModel()
        {
            _settings = OptimizeProcess.Settings;
            _plotFrame = new Lazy<ChromiumWebBrowser>();
            InitializeSettingPages();
        }

        private void InitializeSettingPages()
        {
            Optuna.Study.StudySummary[] summaries = new StorageHandler().GetStudySummaries(_settings.Storage.Path);
            _plotSettingPages = new Dictionary<VisualizeType, Lazy<Page>>
            {
                {VisualizeType.ParetoFront, new Lazy<Page>(() => new ParetoFrontPage(summaries))},
                {VisualizeType.OptimizationHistory, new Lazy<Page>(() => new OptimizationHistoryPage(summaries))},
                {VisualizeType.Slice, new Lazy<Page>(() => new SlicePage(summaries))},
                {VisualizeType.Contour, new Lazy<Page>(() => new ContourPage(summaries))},
                {VisualizeType.ParamImportances, new Lazy<Page>(() => new ParamImportancesPage(summaries))},
                {VisualizeType.ParallelCoordinate, new Lazy<Page>(() => new ParallelCoordinatePage(summaries))},
                {VisualizeType.Hypervolume, new Lazy<Page>(() => new HypervolumePage(summaries))},
                {VisualizeType.EDF, new Lazy<Page>(() => new EdfPage(summaries))},
                {VisualizeType.Rank, new Lazy<Page>(() => new RankPage(summaries))},
                {VisualizeType.Timeline, new Lazy<Page>(() => new TimelinePage(summaries))},
                {VisualizeType.TerminatorImprovement, new Lazy<Page>(() => new TerminatorImprovementPage(summaries))}
            };
            PlotSettingsFrame = _plotSettingPages[VisualizeType.ParetoFront].Value;
        }

        internal void SetTargetVisualizeType(VisualizeType visualizeType)
        {
            _targetVisualizeType = visualizeType;
            PlotSettingsFrame = _plotSettingPages[_targetVisualizeType].Value;
            var frameViewModel = (PlotSettingsViewModelBase)PlotSettingsFrame.DataContext;
            frameViewModel.UpdateItems();

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
            SetLoadingPage();
            switch (_targetVisualizeType)
            {
                case VisualizeType.OptunaHub:
                    break;
                default:
                    PlotAsync();
                    break;
            }
        }

        private void SetLoadingPage()
        {
            string resource = "Tunny.WPF.Assets.html.loading.html";
            string content = LoadEmbeddedHtml(resource);
            PlotFrame.LoadHtml(content);
        }

        private async void PlotAsync()
        {
            var viewModel = (IPlotSettings)PlotSettingsFrame.DataContext;
            PlotSettings plotSettings = viewModel.GetPlotSettings();
            await Task.Run(() =>
            {
                var vis = new VisualizeProcess();
                string htmlPath = vis.Plot(_settings.Storage, plotSettings);
                string fileUrl = "file:///" + htmlPath.Replace("\\", "/");
                PlotFrame.Load(fileUrl);
            });
        }
    }
}
