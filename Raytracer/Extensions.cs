using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raytracer.CustomMath;

namespace Raytracer
{
    public static class Extensions
    {

        public static int Clamp(this int value, int min, int max)
        {
            return Math.Min(max, Math.Max(value, min));
        }

        public static Vector3D RandomInsideUnitDisk(this Random random)
        {
            Vector3D dir = new Vector3D(1.0, 1.0, 0);
            Vector3D point;
            do
            {
                point = 2.0 * new Vector3D(random.NextDouble(), random.NextDouble(), 0.0d) - dir;
            } while (point.Dot(point) >= 1.0);

            return point;
        }

    }
}
