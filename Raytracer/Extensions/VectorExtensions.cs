using System;
using Raytracer.Extensions;
using Raytracer.Types;

namespace Raytracer.Extensions
{
    public static class VectorExtensions
    {
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Min(max, Math.Max(value, min));
        }

    }
}