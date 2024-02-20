using Tunny.Core.Util;

namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.TPESampler.html
    /// </summary>
    public class Tpe
    {
        public int? Seed { get; set; }
        public bool ConsiderPrior { get; set; } = true;
        public double PriorWeight { get; set; } = 1.0;
        public bool ConsiderMagicClip { get; set; } = true;
        public bool ConsiderEndpoints { get; set; }
        public int NStartupTrials { get; set; } = 10;
        public int NEICandidates { get; set; } = 24;
        public bool Multivariate { get; set; }
        public bool Group { get; set; }
        public bool WarnIndependentSampling { get; set; } = true;
        public bool ConstantLiar { get; set; }

        public dynamic ToOptuna(dynamic optuna, bool hasConstraints)
        {
            TLog.MethodStart();
            return optuna.samplers.TPESampler(
                seed: Seed,
                consider_prior: ConsiderPrior,
                prior_weight: 1.0,
                consider_magic_clip: ConsiderMagicClip,
                consider_endpoints: ConsiderEndpoints,
                n_startup_trials: NStartupTrials,
                n_ei_candidates: NEICandidates,
                multivariate: Multivariate,
                group: Group,
                warn_independent_sampling: WarnIndependentSampling,
                constant_liar: ConstantLiar,
                constraints_func: hasConstraints ? SamplerSettings.ConstraintFunc() : null
            );
        }
    }
}
