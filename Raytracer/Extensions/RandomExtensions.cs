using System;
using Raytracer.Types;

namespace Raytracer.Extensions
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random)
        {
            return (float) random.NextDouble();
        }

        public static Vector3D RandomInsideUnitDisk(this Random random)
        {
            var dir = new Vector3D(1, 1, 0);
            Vector3D point;
            do
            {
                point = 2.0 * new Vector3D(random.NextDouble(), random.NextDouble(), 0) - dir;
            } while (point.LengthSquared() >= 1);

            return point;
        }

        public static Vector3D RandomUnitVector(this Random random)
        {
            double z = random.NextDouble() * 2 - 1;
            double r = Math.Sqrt(1 - z * z);
            double a = random.NextDouble() * 2 * Math.PI;
            
            return new Vector3D(r * Math.Cos(a), r * Math.Sin(a), z);
        }

    }
}