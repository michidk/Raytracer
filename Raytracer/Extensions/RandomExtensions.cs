using System;

namespace Raytracer.Extensions
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random)
        {
            return (float) random.NextDouble();
        }
    }
}