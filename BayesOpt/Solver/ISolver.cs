using System;
using System.Collections.Generic;

using BayesOpt.Util;

namespace BayesOpt.Solver
{
    internal interface ISolver
    {
        double[] XOpt { get; }
        double[] FxOpt { get; }
        bool RunSolver(
            List<Variable> variables, Func<IList<decimal>,
            List<double>> evaluate, string preset, string expertsettings,
            string installFolder, string documentPath);

        string GetErrorMessage();

        IEnumerable<string> GetPresetNames();
    }
}