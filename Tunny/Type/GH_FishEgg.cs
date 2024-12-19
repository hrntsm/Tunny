﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using GH_IO.Serialization;

using Grasshopper.Kernel.Types;

namespace Tunny.Type
{
    public class GH_FishEgg : GH_Goo<FishEgg>
    {
        public GH_FishEgg()
        {
        }

        public GH_FishEgg(FishEgg internalData) : base(internalData)
        {
        }

        public GH_FishEgg(GH_Goo<FishEgg> other) : base(other)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "FishEgg";
        public override string TypeDescription => "Goo for Tunny fish egg";
        public override IGH_Goo Duplicate() => new GH_FishEgg { Value = Value };

        public override bool Read(GH_IReader reader)
        {
            string base64 = string.Empty;
            if (reader.TryGetString("FishEgg_bin", ref base64))
            {
                Value = FromBase64(base64);
            }
            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("FishEgg_bin", ToBase64(Value));
            return base.Write(writer);
        }

        public override bool CastFrom(object source)
        {
            if (source is FishEgg eggs)
            {
                Value = eggs;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool CastTo<T>(ref T target)
        {
            target = default;
            if (typeof(T).IsAssignableFrom(typeof(FishEgg)))
            {
                target = (T)(object)Value;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static FishEgg FromBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(bytes))
            {
                return (FishEgg)new BinaryFormatter().Deserialize(ms);
            }
        }

        private static string ToBase64(FishEgg value)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
