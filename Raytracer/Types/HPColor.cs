using System;

namespace Raytracer.Types
{
    // high precision HPColor used for calculations during rendering
    public struct HPColor : IEquatable<HPColor>
    {
        public double Red;
        public double Green;
        public double Blue;

        public HPColor(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public HPColor Normalize()
        {
            var length = Math.Sqrt(Math.Pow(Red, 2) + Math.Pow(Green, 2) + Math.Pow(Blue, 2));
            return new HPColor(Red / length, Green / length, Blue / length);
        }

        public bool Equals(HPColor other)
        {
            return Red.Equals(other.Red) && Green.Equals(other.Green) && Blue.Equals(other.Blue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is HPColor && Equals((HPColor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Red.GetHashCode();
                hashCode = (hashCode * 397) ^ Green.GetHashCode();
                hashCode = (hashCode * 397) ^ Blue.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(HPColor value1, HPColor value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(HPColor value1, HPColor value2)
        {
            return !value1.Equals(value2);
        }

        public static HPColor operator +(HPColor value1, HPColor value2)
        {
            return new HPColor(value1.Red + value2.Red, value1.Green + value2.Green, value1.Blue + value2.Blue);
        }

        public static HPColor operator -(HPColor value1, HPColor value2)
        {
            return new HPColor(value1.Red - value2.Red, value1.Green - value2.Green, value1.Blue - value2.Blue);
        }

        public static HPColor operator *(HPColor value1, double value2)
        {
            return new HPColor(value1.Red * value2, value1.Green * value2, value1.Blue * value2);
        }

        public static HPColor operator /(HPColor value1, double value2)
        {
            return new HPColor(value1.Red / value2, value1.Green / value2, value1.Blue / value2);
        }

        public static HPColor FromSysColor(System.Drawing.Color color)
        {
            return new HPColor(color.R / (double) Color.MAX, color.G / (double) Color.MAX,
                color.B / (double) Color.MAX);
        }

        public static HPColor FromColor(Color color)
        {
            return new HPColor(color.Red / (double) Color.MAX, color.Green / (double) Color.MAX,
                color.Blue / (double) Color.MAX);
        }
    }
}