using System.Collections.Generic;
using System.Drawing;

using Rhino.Geometry;

namespace Tunny.PreProcess
{
    public class Artifact
    {
        public List<GeometryBase> Geometries { get; set; }
        public List<Bitmap> Images { get; set; }

        public int ArtifactCount()
        {
            return Geometries.Count + Images.Count;
        }
    }
}
