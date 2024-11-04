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
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages.Visualize;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal class VisualizeViewModel : BindableBase
    {
        private readonly TSettings _settings;
        private VisualizeType _targetVisualizeType;
        private Dictionary<VisualizeType, Page> _plotSettingPages;

        private ChromiumWebBrowser _plotFrame;
        public ChromiumWebBrowser PlotFrame { get => _plotFrame; set => SetProperty(ref _plotFrame, value); }
        private Page _plotSettingsFrame;
        public Page PlotSettingsFrame { get => _plotSettingsFrame; set => SetProperty(ref _plotSettingsFrame, value); }

        public VisualizeViewModel()
        {
            _settings = OptimizeProcess.Settings;
            PlotFrame = new ChromiumWebBrowser();
            InitializeSettingPages();
        }

        private void InitializeSettingPages()
        {
            _plotSettingPages = new Dictionary<VisualizeType, Page>
            {
                {VisualizeType.ParetoFront, new ParetoFrontPage()},
                {VisualizeType.OptimizationHistory, new OptimizationHistoryPage()},
                {VisualizeType.Slice, new SlicePage()},
                {VisualizeType.Contour, new ContourPage()},
                {VisualizeType.ParamImportances, new ParamImportancesPage()},
                {VisualizeType.ParallelCoordinate, new ParallelCoordinatePage()},
                {VisualizeType.Hypervolume, new HypervolumePage()},
                {VisualizeType.EDF, new EdfPage()},
                {VisualizeType.Rank, new RankPage()},
                {VisualizeType.Timeline, new TimelinePage()},
                {VisualizeType.TerminatorImprovement, new TerminatorImprovementPage()}
            };
            PlotSettingsFrame = _plotSettingPages[VisualizeType.ParetoFront];
        }

        internal void SetTargetVisualizeType(VisualizeType visualizeType)
        {
            _targetVisualizeType = visualizeType;
            PlotSettingsFrame = _plotSettingPages[_targetVisualizeType];
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
