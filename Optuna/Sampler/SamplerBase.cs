using Python.Runtime;

namespace Optuna.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/samplers.html
    /// </summary>
    public class SamplerBase
    {
        public int? Seed { get; set; }
        public const string ConstraintKey = "Constraint";

        public static dynamic ConstraintFunc()
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def constraints(trial):\n" +
                $"  return trial.user_attrs[\"{ConstraintKey}\"]\n"
            );
            return ps.Get("constraints");
        }
    }
}
