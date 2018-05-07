using static System.Math;

namespace Raytracer.CustomMath
{
    public struct Vector3D
    {
        
        // basic vectors
        public static readonly Vector3D ZERO = new Vector3D(0, 0, 0);
        public static readonly Vector3D ONE = new Vector3D(1, 1, 1);
        public static readonly Vector3D X_AXIS = new Vector3D(1, 0, 0);
        public static readonly Vector3D Y_AXIS = new Vector3D(0, 1, 0);
        public static readonly Vector3D Z_AXIS = new Vector3D(0, 0, 1);
        
        // directional vectors
        public static readonly Vector3D UP = Y_AXIS;
        public static readonly Vector3D DOWN = -UP;
        public static readonly Vector3D RIGHT = X_AXIS;
        public static readonly Vector3D LEFT = -LEFT;

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

        public Vector3D Cross(Vector3D other)
        {
            return new Vector3D(
                this.Y * other.Z - this.Z * other.Y,
                this.Z * other.X - this.X * other.Z,
                this.X * other.Y - this.Y * other.X
                );
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

        public static Vector3D operator /(Vector3D value1, double value2)
        {
            return new Vector3D(value1.X / value2, value1.Y / value2, value1.Z / value2);
        }

        public static Vector3D operator /(double value1, Vector3D value2)
        {
            return new Vector3D(value1 / value2.X, value1 / value2.Y, value1 / value2.Z);
        }

    }
}
