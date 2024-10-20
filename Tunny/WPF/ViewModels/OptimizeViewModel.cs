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
                "BayesianOptimization(TPE)",
                "BayesianOptimization(GP:Optuna)",
                "BayesianOptimization(GP:Botorch)",
                "GeneticAlgorithm(NSGA-II)",
                "GeneticAlgorithm(NSGA-III)",
                "EvolutionStrategy(CMA-ES)",
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
