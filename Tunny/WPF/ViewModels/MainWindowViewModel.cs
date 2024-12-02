using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;

using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.ViewModels.Optimize;
using Tunny.WPF.ViewModels.Visualize;
using Tunny.WPF.Views.Pages;
using Tunny.WPF.Views.Pages.Expert;
using Tunny.WPF.Views.Pages.Optimize;
using Tunny.WPF.Views.Pages.Visualize;
using Tunny.WPF.Views.Windows;

namespace Tunny.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase, IDisposable
    {
        private readonly OptimizePage _optimizePage;
        private readonly OptimizeViewModel _optimizeViewModel;
        private readonly VisualizePage _visualizePage;
        private readonly HelpPage _helpPage;
        private readonly ExpertPage _expertPage;
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
            _expertPage = new ExpertPage();
            IsMultiObjective = OptimizeProcess.Component.GhInOut.IsMultiObjective;
            UpdateTitle();
            ReportProgress("Welcome 🐟Tunny🐟 The next-gen Grasshopper optimization tool ", 0);

            _optimizeViewModel = new OptimizeViewModel();
            _optimizePage = new OptimizePage()
            {
                DataContext = _optimizeViewModel
            };
            MainWindowFrame = _optimizePage;
            _optimizeViewModel.ChangeTargetSampler(OptimizeProcess.Settings.Optimize.SamplerType);

            CheckPruner();
            CheckPythonInstalled();
        }

        private void UpdateTitle()
        {
            string storagePath = OptimizeProcess.Settings.Storage.Path;
            WindowTitle = $"Tunny v{TEnvVariables.Version.ToString(2)} - {storagePath}";
        }

        private static void CheckPruner()
        {
            TLog.MethodStart();
            OptimizeProcess.Settings.Pruner.CheckStatus();
            if (OptimizeProcess.Settings.Pruner.GetPrunerStatus() == PrunerStatus.PathError)
            {
                TunnyMessageBox.Error_PrunerPath();
            }
        }

        private void CheckPythonInstalled()
        {
            TLog.MethodStart();
            string tunnyAssembleVersion = TEnvVariables.Version.ToString();
            if (OptimizeProcess.Settings.CheckPythonLibraries || OptimizeProcess.Settings.Version != tunnyAssembleVersion)
            {
                InstallPython();
                OptimizeProcess.Settings.CheckPythonLibraries = false;
                OptimizeProcess.Settings.Version = tunnyAssembleVersion;
                OptimizeProcess.Settings.Serialize(TEnvVariables.OptimizeSettingsPath);
            }
        }

        private DelegateCommand<VisualizeType?> _selectVisualizeTypeCommand;
        public ICommand SelectVisualizeTypeCommand
        {
            get
            {
                if (_selectVisualizeTypeCommand == null)
                {
                    _selectVisualizeTypeCommand = new DelegateCommand<VisualizeType?>(SelectVisualizeType);
                }

                return _selectVisualizeTypeCommand;
            }
        }

        private void SelectVisualizeType(VisualizeType? visualizeType)
        {
            if (visualizeType == null)
            {
                return;
            }
            var viewModel = (VisualizeViewModel)_visualizePage.DataContext;
            viewModel.SetTargetVisualizeType(visualizeType.Value);
            MainWindowFrame = _visualizePage;
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
                case SelectSamplerType.GpPreferential:
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
                case SelectSamplerType.AUTO:
                    samplerType = SamplerType.AUTO;
                    break;
                case SelectSamplerType.MOEAD:
                    samplerType = SamplerType.MOEAD;
                    break;
                case SelectSamplerType.MoCmaEs:
                    samplerType = SamplerType.MoCmaEs;
                    break;
                default:
                    samplerType = SamplerType.TPE;
                    break;
            }
            _optimizeViewModel.ChangeTargetSampler(samplerType);
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

        private DelegateCommand _quickAccessSettingsSaveCommand;
        public ICommand QuickAccessSettingsSaveCommand
        {
            get
            {
                if (_quickAccessSettingsSaveCommand == null)
                {
                    _quickAccessSettingsSaveCommand = new DelegateCommand(QuickAccessSettingsFileSave);
                }

                return _quickAccessSettingsSaveCommand;
            }
        }

        private void QuickAccessSettingsFileSave()
        {
            OptimizeProcess.Settings.Optimize = _optimizeViewModel.GetCurrentSettings();
            OptimizeProcess.Settings.Serialize(TEnvVariables.OptimizeSettingsPath);
        }

        private DelegateCommand _quickAccessNewStorageFileCommand;
        public ICommand QuickAccessNewStorageFileCommand
        {
            get
            {
                if (_quickAccessNewStorageFileCommand == null)
                {
                    _quickAccessNewStorageFileCommand = new DelegateCommand(QuickAccessNewStorageFile);
                }

                return _quickAccessNewStorageFileCommand;
            }
        }
        private void QuickAccessNewStorageFile()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
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
                if (File.Exists(dialog.FileName) == false)
                {
                    OptimizeProcess.Settings.Storage.CreateNewOptunaStorage(true);
                }
                UpdateTitle();
            }
        }

        private DelegateCommand _quickAccessFileOpenCommand;
        public ICommand QuickAccessStorageFileOpenCommand
        {
            get
            {
                if (_quickAccessFileOpenCommand == null)
                {
                    _quickAccessFileOpenCommand = new DelegateCommand(QuickAccessStorageFileOpen);
                }

                return _quickAccessFileOpenCommand;
            }
        }
        private void QuickAccessStorageFileOpen()
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

        public void Dispose()
        {
            _helpPage.Dispose();
            GC.SuppressFinalize(this);
        }

        private DelegateCommand _installPythonCommand;
        public ICommand InstallPythonCommand
        {
            get
            {
                if (_installPythonCommand == null)
                {
                    _installPythonCommand = new DelegateCommand(InstallPython);
                }

                return _installPythonCommand;
            }
        }
        private async void InstallPython()
        {
            TLog.MethodStart();
            var installer = new PythonInstaller(this);
            _optimizeViewModel.EnableRunOptimizeButton = false;
            await installer.RunAsync();
            _optimizeViewModel.EnableRunOptimizeButton = true;
        }

        private string _statusBarNotifyText;
        public string StatusBarNotifyText { get => _statusBarNotifyText; set => SetProperty(ref _statusBarNotifyText, value); }
        private string _statusBarTrialNum;
        public string StatusBarTrialNum { get => _statusBarTrialNum; set => SetProperty(ref _statusBarTrialNum, value); }
        private string _statusBarETA;
        public string StatusBarETA { get => _statusBarETA; set => SetProperty(ref _statusBarETA, value); }
        private double _statusBarProgress;
        public double StatusBarProgress { get => _statusBarProgress; set => SetProperty(ref _statusBarProgress, value); }

        public void ReportProgress(string notifyText, double progress)
        {
            StatusBarNotifyText = notifyText;
            StatusBarProgress = progress;
        }

        public void ReportProgress(string notifyText, string trialNum, double progress)
        {
            StatusBarNotifyText = notifyText;
            StatusBarTrialNum = trialNum;
            StatusBarProgress = progress;
        }
    }
}
