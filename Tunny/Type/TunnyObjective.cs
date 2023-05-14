using System.Drawing;

namespace Tunny.Type
{
    public class TunnyObjective
    {
        public double[] Numbers { get; set; }
        public Bitmap[] Images { get; set; }

        public TunnyObjective()
        {
            Numbers = System.Array.Empty<double>();
            Images = System.Array.Empty<Bitmap>();
        }

        public TunnyObjective(double[] numbers, Bitmap[] images)
        {
            Numbers = numbers;
            Images = images;
        }

        public int Length => Numbers.Length + Images.Length;
    }
}
