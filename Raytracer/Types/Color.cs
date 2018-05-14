using System;

namespace Raytracer.Types
{
    public struct Color : IEquatable<Color>
    {
        public double Red;
        public double Green;
        public double Blue;

        public Color(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public Color Normalize()
        {
            var length = Math.Sqrt(Math.Pow(Red, 2) + Math.Pow(Green, 2) + Math.Pow(Blue, 2));
            return new Color(Red / length, Green / length, Blue / length);
        }

        public bool Equals(Color other)
        {
            return Red.Equals(other.Red) && Green.Equals(other.Green) && Blue.Equals(other.Blue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Color && Equals((Color) obj);
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

        public static bool operator ==(Color value1, Color value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Color value1, Color value2)
        {
            return !value1.Equals(value2);
        }

        public static Color operator +(Color value1, Color value2)
        {
            return new Color(value1.Red + value2.Red, value1.Green + value2.Green, value1.Blue + value2.Blue);
        }

        public static Color operator -(Color value1, Color value2)
        {
            return new Color(value1.Red - value2.Red, value1.Green - value2.Green, value1.Blue - value2.Blue);
        }

        public static Color operator *(Color value1, double value2)
        {
            return new Color(value1.Red * value2, value1.Green * value2, value1.Blue * value2);
        }

        public static Color operator *(double value1, Color value2)
        {
            return value2 * value1;
        }

        public static Color operator *(Color value1, Color value2)
        {
            return new Color(value1.Red * value2.Red, value1.Green * value2.Green, value1.Blue * value2.Blue);
        }

        public static Color operator /(Color value1, double value2)
        {
            return new Color(value1.Red / value2, value1.Green / value2, value1.Blue / value2);
        }

        public static Color FromSysColor(System.Drawing.Color color)
        {
            return new Color(color.R / (double) 255, color.G / (double) 255,
                color.B / (double) 255);
        }
    }
}