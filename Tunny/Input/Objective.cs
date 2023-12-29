using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

using Tunny.Type;

namespace Tunny.Input
{
    public class Objective
    {
        public List<IGH_Param> Sources { get; set; }
        public double[] Numbers { get; private set; }
        public Bitmap[] Images { get; private set; }
        public GeometryBase[] Geometries { get; private set; }
        public bool IsHumanInTheLoop { get; private set; }

        public Objective(List<IGH_Param> sources)
        {
            var numbers = new List<double>();
            var images = new List<Bitmap>();
            var geometries = new List<GeometryBase>();
            Sources = sources;

            foreach (IGH_StructureEnumerator ghEnumerator in sources.Select(objective => objective.VolatileData.AllData(false)))
            {
                foreach (IGH_Goo goo in ghEnumerator)
                {
                    bool result = goo.CastTo(out GeometryBase geometry);
                    if (result)
                    {
                        geometries.Add(geometry);
                        continue;
                    }
                    if (goo is GH_FishPrint fishPrint)
                    {
                        images.Add(fishPrint.Value);
                        continue;
                    }
                    result = goo.CastTo(out double number);
                    if (result)
                    {
                        numbers.Add(number);
                    }
                }
            }

            Numbers = numbers.ToArray();
            Images = images.ToArray();
            Geometries = geometries.ToArray();
        }

        public Objective(double[] numbers, Bitmap[] images, GeometryBase[] geometries)
        {
            Numbers = numbers;
            Images = images;
            Geometries = geometries;
        }

        public int Length => Numbers.Length + Images.Length + Geometries.Length > 0 ? 1 : 0;
        public int NoNumberLength => Images.Length + Geometries.Length > 0 ? 1 : 0;

        public string[] GetNickNames()
        {
            var nickNames = new List<string>();
            foreach (IGH_Param source in Sources)
            {
                nickNames.Add(source.NickName);
            }
            return nickNames.ToArray();
        }
    }
}
