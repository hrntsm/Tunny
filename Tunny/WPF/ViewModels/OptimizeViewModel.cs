using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Tunny.WPF.ViewModels
{
    public class OptimizeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _samplers;
        public ObservableCollection<string> Samplers
        {
            get { return _samplers; }
            set
            {
                _samplers = value;
                OnPropertyChanged(nameof(Samplers));
            }
        }

        private string _selectedSampler;
        public string SelectedSampler
        {
            get { return _selectedSampler; }
            set
            {
                _selectedSampler = value;
                OnPropertyChanged(nameof(SelectedSampler));
            }
        }

        public OptimizeViewModel()
        {
            Samplers = new ObservableCollection<string>
            {
                "BO-TPE",
                "BO-GP:Optuna",
                "BO-GP:Botorch",
                "NSGA-II",
                "NSGA-III",
                "CMA-ES",
                "Quasi-MonteCarlo",
                "Random",
                "BruteForce"
            };

            SelectedSampler = Samplers[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
