﻿using System.Linq;

using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    class HypervolumeViewModel : PlotSettingsViewModelBase
    {
        private string _referencePoint;
        public string ReferencePoint { get => _referencePoint; set => SetProperty(ref _referencePoint, value); }

        public HypervolumeViewModel()
        {
            ReferencePoint = "AUTO";
        }

        public override PlotSettings GetPlotSettings()
        {
            double[] referencePoint = string.IsNullOrEmpty(ReferencePoint) || ReferencePoint.Equals("AUTO", System.StringComparison.OrdinalIgnoreCase)
                ? null
                : ReferencePoint.Split(',').Select(double.Parse).ToArray();
            return new PlotSettings()
            {
                TargetStudyName = SelectedStudyName.Name,
                PlotTypeName = "hypervolume",
                ReferencePoint = referencePoint
            };
        }
    }
}