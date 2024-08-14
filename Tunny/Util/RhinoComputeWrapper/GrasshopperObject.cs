using Newtonsoft.Json;

namespace Tunny.Util.RhinoComputeWrapper
{
    public class GrasshopperObject
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        [JsonConstructor]
        public GrasshopperObject()
        {
        }

        public GrasshopperObject(object obj)
        {
            Data = JsonConvert.SerializeObject(obj, GeometryResolver.Settings);
            Type = obj.GetType().FullName;
        }

        /// <summary>
        /// Used internally for RestHopperObject serialization
        /// </summary>
        class GeometryResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            static JsonSerializerSettings _settings;
            public static JsonSerializerSettings Settings
            {
                get
                {
                    if (_settings == null)
                    {
                        _settings = new JsonSerializerSettings { ContractResolver = new GeometryResolver() };
                        // return V6 ON_Objects for now
                        var options = new Rhino.FileIO.SerializationOptions
                        {
                            RhinoVersion = 6,
                            WriteUserData = true
                        };
                        _settings.Context = new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.All, options);
                        //_settings.Converters.Add(new ArchivableDictionaryResolver());
                    }
                    return _settings;
                }
            }

            protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
            {
                Newtonsoft.Json.Serialization.JsonProperty property = base.CreateProperty(member, memberSerialization);
                if (property.DeclaringType == typeof(Rhino.Geometry.Circle))
                {
                    property.ShouldSerialize = _ => property.PropertyName != "IsValid" && property.PropertyName != "BoundingBox" && property.PropertyName != "Diameter" && property.PropertyName != "Circumference";

                }
                if (property.DeclaringType == typeof(Rhino.Geometry.Plane))
                {
                    property.ShouldSerialize = _ => property.PropertyName != "IsValid" && property.PropertyName != "OriginX" && property.PropertyName != "OriginY" && property.PropertyName != "OriginZ";
                }

                if (property.DeclaringType == typeof(Rhino.Geometry.Point3f) ||
                    property.DeclaringType == typeof(Rhino.Geometry.Point2f) ||
                    property.DeclaringType == typeof(Rhino.Geometry.Vector2f) ||
                    property.DeclaringType == typeof(Rhino.Geometry.Vector3f))
                {
                    property.ShouldSerialize = _ => property.PropertyName == "X" || property.PropertyName == "Y" || property.PropertyName == "Z";
                }

                if (property.DeclaringType == typeof(Rhino.Geometry.Line))
                {
                    property.ShouldSerialize = _ => property.PropertyName == "From" || property.PropertyName == "To";
                }

                if (property.DeclaringType == typeof(Rhino.Geometry.MeshFace))
                {
                    property.ShouldSerialize = _ => property.PropertyName != "IsTriangle" && property.PropertyName != "IsQuad";
                }
                return property;
            }
        }
    }
}

