using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer.CustomMath
{
    public struct Size2D : IEquatable<Size2D>
    {

        public static Size2D Zero = new Size2D(0, 0);

        public int Width;
        public int Height;

        public int Area => Width * Height;

        public Size2D(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(Size2D other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Size2D && Equals((Size2D) obj);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        public static bool operator ==(Size2D value1, Size2D value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Size2D value1, Size2D value2)
        {
            return !value1.Equals(value2);
        }

    }
}
