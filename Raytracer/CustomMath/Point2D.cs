using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer.CustomMath
{
    public struct Point2D : IEquatable<Point2D>
    {

        public int X;
        public int Y;

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public bool Equals(Point2D other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point2D && Equals((Point2D) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Point2D value1, Point2D value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Point2D value1, Point2D value2)
        {
            return !value1.Equals(value2);
        }

    }
}
