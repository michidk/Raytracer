using static System.Math;

namespace Raytracer.CustomMath
{
    public struct Vector3D
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Dot(Vector3D other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public double LengthSquared()
        {
            return Pow(X, 2) + Pow(Y, 2) + Pow(Z, 2);
        }

        public double Length()
        {
            return Sqrt(LengthSquared());
        }

        public Vector3D Normalize()
        {
            var length = Length();
            return new Vector3D(X / length, Y / length, Z / length);
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

        public static bool operator ==(Vector3D value1, Vector3D value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Vector3D value1, Vector3D value2)
        {
            return !value1.Equals(value2);
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

        public static Vector3D operator /(Vector3D value1, double value2)
        {
            return new Vector3D(value1.X / value2, value1.Y / value2, value1.Z / value2);
        }

    }
}
