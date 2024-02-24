using System.Collections.Generic;
using System.Linq;

using Optuna.Trial;

using Tunny.Core.Util;

namespace Tunny.Core.PostProcess
{
    public class ModelResult
    {
        public int Number { get; set; }
        public double[] Objectives { get; set; }
        public Dictionary<string, object> Variables { get; set; }
        public Dictionary<string, List<string>> Attributes { get; set; }

        public ModelResult(Trial trial)
        {
            TLog.MethodStart();
            Number = trial.Number;
            Variables = trial.Params;
            Objectives = trial.Values;
            Attributes = ParseAttrs(trial.UserAttrs);
        }

        private static Dictionary<string, List<string>> ParseAttrs(Dictionary<string, object> userAttrs)
        {
            TLog.MethodStart();
            var attributes = new Dictionary<string, List<string>>();
            foreach (string key in userAttrs.Keys)
            {
                string[] values = userAttrs[key] as string[];
                attributes.Add(key, values.ToList());
            }
            return attributes;
        }
    }
}
