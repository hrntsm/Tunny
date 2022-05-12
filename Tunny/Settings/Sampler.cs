namespace Tunny.Settings
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/samplers.html
    /// </summary>
    public class Sampler
    {
        public Random Random { get; set; } = new Random();
        public Tpe Tpe { get; set; } = new Tpe();
        public CmaEs CmaEs { get; set; } = new CmaEs();
        public NSGAII NsgaII { get; set; } = new NSGAII();
    }
}
