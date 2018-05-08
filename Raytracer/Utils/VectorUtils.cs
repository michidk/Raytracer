using System;
using Raytracer.Extensions;
using Raytracer.Types;

namespace Raytracer.Utils
{
    public static class VectorUtils
    {
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Min(max, Math.Max(value, min));
        }

        public static Vector3D RandomInsideUnitDisk(this Random random)
        {
            var dir = new Vector3D(1, 1, 0);
            Vector3D point;
            do
            {
                point = 2 * new Vector3D(random.NextFloat(), random.NextFloat(), 0) - dir;
            } while (point.Dot(point) >= 1);

            return point;
        }
    }
}