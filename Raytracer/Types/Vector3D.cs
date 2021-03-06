﻿using static System.Math;

namespace Raytracer.Types
{
    public struct Vector3D
    {
        // basic vectors
        public static readonly Vector3D Zero = new Vector3D(0, 0, 0);
        public static readonly Vector3D One = new Vector3D(1, 1, 1);
        public static readonly Vector3D XAxis = new Vector3D(1, 0, 0);
        public static readonly Vector3D YAxis = new Vector3D(0, 1, 0);
        public static readonly Vector3D ZAxis = new Vector3D(0, 0, 1);

        // directional vectors
        public static readonly Vector3D Up = YAxis;
        public static readonly Vector3D Down = -Up;
        public static readonly Vector3D Right = XAxis;
        public static readonly Vector3D Left = -Right;
        public static readonly Vector3D Forward = ZAxis;
        public static readonly Vector3D Backward = -Forward;

        // members
        public double X;
        public double Y;
        public double Z;

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D Hadamard(Vector3D other)
        {
            return new Vector3D(X * other.X, Y * other.Y, Z * other.Z);
        }

        public double Dot(Vector3D other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public Vector3D Cross(Vector3D other)
        {
            return new Vector3D(
                Y * other.Z - Z * other.Y,
                Z * other.X - X * other.Z,
                X * other.Y - Y * other.X
            );
        }

        public double LengthSquared()
        {
            return Dot(this); //Pow(X, 2) + Pow(Y, 2) + Pow(Z, 2);
        }

        public double Length()
        {
            return Sqrt(LengthSquared());
        }

        public Vector3D Normalize()
        {
            var length = Length();
            if (length == 0)
                return Zero;

            return new Vector3D(X / length, Y / length, Z / length);
        }

        public static bool operator ==(Vector3D value1, Vector3D value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Vector3D value1, Vector3D value2)
        {
            return !(value1 == value2);
        }

        public static Vector3D operator +(Vector3D value)
        {
            return new Vector3D(value.X, value.Y, value.Z);
        }

        public static Vector3D operator -(Vector3D value)
        {
            return new Vector3D(-value.X, -value.Y, -value.Z);
        }

        public static Vector3D operator +(Vector3D value1, Vector3D value2)
        {
            return new Vector3D(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);
        }

        public static Vector3D operator -(Vector3D value1, Vector3D value2)
        {
            return new Vector3D(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
        }

        public static Vector3D operator *(Vector3D value1, double value2)
        {
            return new Vector3D(value1.X * value2, value1.Y * value2, value1.Z * value2);
        }

        public static Vector3D operator *(double value1, Vector3D value2)
        {
            return value2 * value1;
        }

        public static Vector3D operator *(Vector3D value1, Vector3D value2)
        {
            return value1.Hadamard(value2);
        }

        public static Vector3D operator /(Vector3D value1, double value2)
        {
            return new Vector3D(value1.X / value2, value1.Y / value2, value1.Z / value2);
        }

        public static Vector3D operator /(double value1, Vector3D value2)
        {
            return new Vector3D(value1 / value2.X, value1 / value2.Y, value1 / value2.Z);
        }

        public bool Equals(Vector3D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector3D && Equals((Vector3D) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}