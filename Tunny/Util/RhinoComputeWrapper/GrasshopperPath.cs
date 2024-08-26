using System.Collections.Generic;

namespace Tunny.Util.RhinoComputeWrapper
{
    public class GrasshopperPath
    {
        public int[] Path
        {
            get; set;
        }

        public GrasshopperPath()
        {
        }

        public GrasshopperPath(int path)
        {
            Path = new int[] { path };
        }

        public GrasshopperPath(int[] path)
        {
            Path = path;
        }

        public GrasshopperPath(string path)
        {
            Path = FromString(path);
        }

        public override string ToString()
        {
            string sPath = "{ ";
            foreach (int i in Path)
            {
                sPath += $"{i}; ";
            }
            sPath += "}";
            return sPath;
        }

        public static int[] FromString(string path)
        {
            string primer = path.Replace(" ", "").Replace("{", "").Replace("}", "");
            string[] stringValues = primer.Split(';');
            var ints = new List<int>();
            foreach (string s in stringValues)
            {
                if (s != string.Empty)
                {
                    ints.Add(int.Parse(s, System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            return ints.ToArray();
        }

        public GrasshopperPath(GrasshopperPath pathObj, int i)
        {
            int[] path = pathObj.Path;
            Path = new int[path.Length + 1];

            for (int j = 0; j < path.Length; j++)
            {
                Path[j] = path[j];
            }
            Path[path.Length] = i;
        }
    }
}

