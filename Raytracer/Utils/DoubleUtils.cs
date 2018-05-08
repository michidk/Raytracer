using System;

namespace Raytracer.Utils
{
    public static class DoubleUtils
    {

        public static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

    }
}