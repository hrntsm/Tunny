using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages;
using Tunny.WPF.Views.Pages.Optimize;
using Tunny.WPF.Views.Pages.Visualize;
using Tunny.WPF.Views.Windows;

namespace Tunny.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly OptimizePage _optimizePage;
        private readonly VisualizePage _visualizePage;
        private readonly HelpPage _helpPage;
        public bool IsSingleObjective { get => !_isMultiObjective; }
        private bool _isMultiObjective;
        public bool IsMultiObjective { get => _isMultiObjective; set => SetProperty(ref _isMultiObjective, value); }
        private Page _mainWindowFrame;
        public Page MainWindowFrame { get => _mainWindowFrame; set => SetProperty(ref _mainWindowFrame, value); }
        private string _windowTitle;
        public string WindowTitle { get => _windowTitle; set => SetProperty(ref _windowTitle, value); }

        public MainWindowViewModel()
        {
            _visualizePage = new VisualizePage();
            _helpPage = new HelpPage();
            IsMultiObjective = OptimizeProcess.Component.GhInOut.IsMultiObjective;
            UpdateTitle();

            _optimizePage = new OptimizePage();
            _optimizePage.ChangeTargetSampler(OptimizeProcess.Settings.Optimize.SamplerType);
            MainWindowFrame = _optimizePage;
        }

        private void UpdateTitle()
        {
            string storagePath = OptimizeProcess.Settings.Storage.Path;
            WindowTitle = $"Tunny v{TEnvVariables.Version.ToString(2)} - {storagePath}";
        }

        private void SetVisualizeType(VisualizeType visualizeType)
        {
            _visualizePage.SetTargetVisualizeType(visualizeType);
            MainWindowFrame = _visualizePage;
        }

        private DelegateCommand _plotParetoFrontCommand;
        public ICommand PlotParetoFrontCommand
        {
            get
            {
                if (_plotParetoFrontCommand == null)
                {
                    _plotParetoFrontCommand = new DelegateCommand(PlotParetoFront);
                }

                return _plotParetoFrontCommand;
            }
        }
        private void PlotParetoFront()
        {
            SetVisualizeType(VisualizeType.ParetoFront);
        }

        private DelegateCommand _plotHistoryCommand;
        public ICommand PlotHistoryCommand
        {
            get
            {
                if (_plotHistoryCommand == null)
                {
                    _plotHistoryCommand = new DelegateCommand(PlotHistory);
                }

                return _plotHistoryCommand;
            }
        }
        private void PlotHistory()
        {
        }

        private DelegateCommand _plotSliceCommand;
        public ICommand PlotSliceCommand
        {
            get
            {
                if (_plotSliceCommand == null)
                {
                    _plotSliceCommand = new DelegateCommand(PlotSlice);
                }

                return _plotSliceCommand;
            }
        }
        private void PlotSlice()
        {
        }

        private DelegateCommand _plotContourCommand;
        public ICommand PlotContourCommand
        {
            get
            {
                if (_plotContourCommand == null)
                {
                    _plotContourCommand = new DelegateCommand(PlotContour);
                }

                return _plotContourCommand;
            }
        }
        private void PlotContour()
        {
        }

        private DelegateCommand _plotParameterImportanceCommand;
        public ICommand PlotParameterImportanceCommand
        {
            get
            {
                if (_plotParameterImportanceCommand == null)
                {
                    _plotParameterImportanceCommand = new DelegateCommand(PlotParameterImportance);
                }

                return _plotParameterImportanceCommand;
            }
        }
        private void PlotParameterImportance()
        {
        }

        private DelegateCommand _plotParallelCoordinateCommand;
        public ICommand PlotParallelCoordinateCommand
        {
            get
            {
                if (_plotParallelCoordinateCommand == null)
                {
                    _plotParallelCoordinateCommand = new DelegateCommand(PlotParallelCoordinate);
                }

                return _plotParallelCoordinateCommand;
            }
        }
        private void PlotParallelCoordinate()
        {
        }

        private DelegateCommand _plotHyperVolumeCommand;
        public ICommand PlotHyperVolumeCommand
        {
            get
            {
                if (_plotHyperVolumeCommand == null)
                {
                    _plotHyperVolumeCommand = new DelegateCommand(PlotHyperVolume);
                }

                return _plotHyperVolumeCommand;
            }
        }
        private void PlotHyperVolume()
        {
        }

        private DelegateCommand _plotEdfCommand;
        public ICommand PlotEdfCommand
        {
            get
            {
                if (_plotEdfCommand == null)
                {
                    _plotEdfCommand = new DelegateCommand(PlotEdf);
                }

                return _plotEdfCommand;
            }
        }
        private void PlotEdf()
        {
        }

        private DelegateCommand _plotRankCommand;
        public ICommand PlotRankCommand
        {
            get
            {
                if (_plotRankCommand == null)
                {
                    _plotRankCommand = new DelegateCommand(PlotRank);
                }

                return _plotRankCommand;
            }
        }
        private void PlotRank()
        {
        }

        private DelegateCommand _plotTimelineCommand;
        public ICommand PlotTimelineCommand
        {
            get
            {
                if (_plotTimelineCommand == null)
                {
                    _plotTimelineCommand = new DelegateCommand(PlotTimeline);
                }

                return _plotTimelineCommand;
            }
        }
        private void PlotTimeline()
        {
        }

        private DelegateCommand _plotTerminatorImprovementCommand;
        public ICommand PlotTerminatorImprovementCommand
        {
            get
            {
                if (_plotTerminatorImprovementCommand == null)
                {
                    _plotTerminatorImprovementCommand = new DelegateCommand(PlotTerminatorImprovement);
                }

                return _plotTerminatorImprovementCommand;
            }
        }
        private void PlotTerminatorImprovement()
        {
        }

        private DelegateCommand _registerOptunaHubSamplerCommand;

        public ICommand RegisterOptunaHubSamplerCommand
        {
            get
            {
                if (_registerOptunaHubSamplerCommand == null)
                {
                    _registerOptunaHubSamplerCommand = new DelegateCommand(RegisterOptunaHubSampler);
                }

                return _registerOptunaHubSamplerCommand;
            }
        }

        private void RegisterOptunaHubSampler()
        {
        }

        private DelegateCommand<SelectSamplerType?> _selectSamplerCommand;
        public ICommand SelectSamplerCommand
        {
            get
            {
                if (_selectSamplerCommand == null)
                {
                    _selectSamplerCommand = new DelegateCommand<SelectSamplerType?>(SelectSampler);
                }

                return _selectSamplerCommand;
            }
        }

        private void SelectSampler(SelectSamplerType? selectSamplerType)
        {
            SamplerType samplerType;
            switch (selectSamplerType)
            {
                case SelectSamplerType.AUTO:
                case SelectSamplerType.GpPreferential:
                case SelectSamplerType.MoCmaEs:
                case SelectSamplerType.MOEAD:
                case null:
                    return;
                case SelectSamplerType.TPE:
                    samplerType = SamplerType.TPE;
                    break;
                case SelectSamplerType.GpOptuna:
                    samplerType = SamplerType.GP;
                    break;
                case SelectSamplerType.GpBoTorch:
                    samplerType = SamplerType.BoTorch;
                    break;
                case SelectSamplerType.NSGAII:
                    samplerType = SamplerType.NSGAII;
                    break;
                case SelectSamplerType.NSGAIII:
                    samplerType = SamplerType.NSGAIII;
                    break;
                case SelectSamplerType.CmaEs:
                    samplerType = SamplerType.CmaEs;
                    break;
                case SelectSamplerType.Random:
                    samplerType = SamplerType.Random;
                    break;
                case SelectSamplerType.QMC:
                    samplerType = SamplerType.QMC;
                    break;
                case SelectSamplerType.BruteForce:
                    samplerType = SamplerType.BruteForce;
                    break;
                default:
                    samplerType = SamplerType.TPE;
                    break;
            }
            _optimizePage.ChangeTargetSampler(samplerType);
            OptimizeProcess.Settings.Optimize.SamplerType = samplerType;
            MainWindowFrame = _optimizePage;
        }

        private DelegateCommand<HelpType?> _openHelpPageCommand;
        public ICommand OpenHelpPageCommand
        {
            get
            {
                if (_openHelpPageCommand == null)
                {
                    _openHelpPageCommand = new DelegateCommand<HelpType?>(OpenHelpPage);
                }

                return _openHelpPageCommand;
            }
        }

        private void OpenHelpPage(HelpType? helpType)
        {
            _helpPage.OpenSite(helpType);
            MainWindowFrame = _helpPage;
        }

        private DelegateCommand _quickAccessFileSaveCommand;
        public ICommand QuickAccessFileSaveCommand
        {
            get
            {
                if (_quickAccessFileSaveCommand == null)
                {
                    _quickAccessFileSaveCommand = new DelegateCommand(QuickAccessFileSave);
                }

                return _quickAccessFileSaveCommand;
            }
        }

        private void QuickAccessFileSave()
        {
            OptimizeProcess.Settings.Optimize = _optimizePage.GetCurrentSettings();
            OptimizeProcess.Settings.Serialize(TEnvVariables.OptimizeSettingsPath);
        }

        private DelegateCommand _quickAccessFileOpenCommand;
        public ICommand QuickAccessFileOpenCommand
        {
            get
            {
                if (_quickAccessFileOpenCommand == null)
                {
                    _quickAccessFileOpenCommand = new DelegateCommand(QuickAccessFileOpen);
                }

                return _quickAccessFileOpenCommand;
            }
        }
        private void QuickAccessFileOpen()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "fish.log",
                DefaultExt = "log",
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite|All Files (*.*)|*.*",
                Title = @"Set Tunny Result File Path",
            };

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                OptimizeProcess.Settings.Storage.Path = dialog.FileName;
                UpdateTitle();
            }
        }

        private DelegateCommand _runOptunaDashboardCommand;
        public ICommand RunOptunaDashboardCommand
        {
            get
            {
                if (_runOptunaDashboardCommand == null)
                {
                    _runOptunaDashboardCommand = new DelegateCommand(RunOptunaDashboard);
                }

                return _runOptunaDashboardCommand;
            }
        }
        private void RunOptunaDashboard()
        {
            TLog.MethodStart();
            if (File.Exists(OptimizeProcess.Settings.Storage.Path) == false)
            {
                TunnyMessageBox.Error_ResultFileNotExist();
                return;
            }
            string dashboardPath = Path.Combine(TEnvVariables.TunnyEnvPath, "python", "Scripts", "optuna-dashboard.exe");
            string storagePath = OptimizeProcess.Settings.Storage.Path;

            var dashboard = new Optuna.Dashboard.Handler(dashboardPath, storagePath);
            dashboard.Run(true);
        }

        private DelegateCommand _runDesignExplorerCommand;
        public ICommand RunDesignExplorerCommand
        {
            get
            {
                if (_runDesignExplorerCommand == null)
                {
                    _runDesignExplorerCommand = new DelegateCommand(RunDesignExplorer);
                }

                return _runDesignExplorerCommand;
            }
        }
        private void RunDesignExplorer()
        {
            TLog.MethodStart();
            var selector = new TargetStudyNameSelector();
            selector.Show();
        }
    }
}
