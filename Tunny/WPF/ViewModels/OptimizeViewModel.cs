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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
