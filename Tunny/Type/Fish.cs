using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Newtonsoft.Json;

using Optuna.Trial;

using Rhino.Geometry;

using Tunny.Core.Input;
using Tunny.Core.Util;

namespace Tunny.Type
{
    [Serializable]
    public class Fish
    {
        public int TrialNumber { get; set; }
        public Dictionary<string, object> Variables { get; set; }
        public Dictionary<string, double> Objectives { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

        public Fish()
        {
            TLog.MethodStart();
        }

        public Fish(Trial trial, string[] objectiveNames)
        {
            TLog.MethodStart();
            TrialNumber = trial.Number;
            Variables = trial.Params;
            Attributes = trial.UserAttrs;
            Objectives = new Dictionary<string, double>();
            for (int i = 0; i < objectiveNames.Length; i++)
            {
                Objectives.Add(objectiveNames[i], trial.Values[i]);
            }
        }

        public List<GeometryBase> GetGeometries()
        {
            TLog.MethodStart();
            var geometries = new List<GeometryBase>();
            if (Attributes == null)
            {
                return geometries;
            }

            bool hasGeometry = Attributes.ContainsKey("Geometry");
            return hasGeometry ? Attributes["Geometry"] as List<GeometryBase> : geometries;
        }

        public string SerializeToJson()
        {
            TLog.MethodStart();
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Fish DeserializeFromJson(string json)
        {
            TLog.MethodStart();
            return JsonConvert.DeserializeObject<Fish>(json);
        }

        public Parameter[] GetParameterClassFormatVariables()
        {
            TLog.MethodStart();
            var parameters = new List<Parameter>();
            if (Variables == null)
            {
                return parameters.ToArray();
            }

            foreach (KeyValuePair<string, object> variable in Variables)
            {
                if (variable.Value is double v)
                {
                    parameters.Add(new Parameter(v));
                }
                else
                {
                    parameters.Add(new Parameter(variable.Value.ToString()));
                }
            }
            return parameters.ToArray();
        }

        public static string ToBase64(Fish fish)
        {
            TLog.MethodStart();
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, fish);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static Fish FromBase64(string base64String)
        {
            TLog.MethodStart();
            byte[] bytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(bytes))
            {
                return (Fish)new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
