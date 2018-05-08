using System;

namespace Raytracer.Types
{
    public struct Color : IEquatable<Color>
    {
        public const byte MAX = 255;

        public byte Red;
        public byte Green;
        public byte Blue;

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool Equals(Color other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue;
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
            return new Color((byte) (value1.Red + value2.Red), (byte) (value1.Green + value2.Green),
                (byte) (value1.Blue + value2.Blue));
        }

        public static Color operator -(Color value1, Color value2)
        {
            return new Color((byte) (value1.Red - value2.Red), (byte) (value1.Green - value2.Green),
                (byte) (value1.Blue - value2.Blue));
        }

        public static Color operator *(Color value1, double value2)
        {
            return new Color((byte) (value1.Red * value2), (byte) (value1.Green * value2),
                (byte) (value1.Blue * value2));
        }

        public static Color operator /(Color value1, double value2)
        {
            return new Color((byte) (value1.Red / value2), (byte) (value1.Green / value2),
                (byte) (value1.Blue / value2));
        }

        public static Color FromSysColor(System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B);
        }

        public static Color FromHPColor(HPColor color)
        {
            return new Color((byte) (color.Red * MAX), (byte) (color.Green * MAX), (byte) (color.Blue * MAX));
        }
    }
}