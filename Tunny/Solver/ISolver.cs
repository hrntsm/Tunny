using System;
using System.Collections.Generic;

using Tunny.Util;

namespace Tunny.Solver
{
    internal interface ISolver
    {
        double[] XOpt { get; }
        double[] FxOpt { get; }
        bool RunSolver(
            List<Variable> variables, Func<IList<decimal>, int, List<double>> evaluate,
            string preset, Dictionary<string, object> settings,
            string installFolder, string documentPath);

        string GetErrorMessage();

        IEnumerable<string> GetPresetNames();
    }
}