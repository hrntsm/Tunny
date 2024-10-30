using Prism.Commands;
using Prism.Mvvm;

using System.Windows.Controls;
using System.Windows.Input;

using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages.Optimize;
using Tunny.WPF.Views.Pages.Visualize;

namespace Tunny.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly OptimizePage _optimizePage;
        private readonly VisualizePage _visualizePage;
        public bool IsSingleObjective { get => !_isMultiObjective; }
        private bool _isMultiObjective;
        public bool IsMultiObjective { get => _isMultiObjective; set => SetProperty(ref _isMultiObjective, value); }
        private Page _mainWindowFrame;
        public Page MainWindowFrame { get => _mainWindowFrame; set => SetProperty(ref _mainWindowFrame, value); }

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(OptimizePage optimizePage, VisualizePage visualizePage)
        {
            _optimizePage = optimizePage;
            _visualizePage = visualizePage;
            IsMultiObjective = OptimizeProcess.Component.GhInOut.IsMultiObjective;
        }

        private void SetSamplerType(Common.SamplerType samplerType)
        {
            _optimizePage.ChangeTargetSampler(samplerType);
            OptimizeProcess.Settings.Optimize.SamplerType = samplerType;
            MainWindowFrame = _optimizePage;
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

        private DelegateCommand _autoSamplerCommand;
        public ICommand AutoSamplerCommand
        {
            get
            {
                if (_autoSamplerCommand == null)
                {
                    _autoSamplerCommand = new DelegateCommand(AutoSampler);
                }

                return _autoSamplerCommand;
            }
        }
        private void AutoSampler()
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
    }
}
