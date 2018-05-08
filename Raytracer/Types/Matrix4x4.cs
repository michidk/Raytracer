using System;

namespace Raytracer.Types
{
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        public const int Rows = 4;
        public const int Columns = 4;

        // Myx | Mrowcolumn
        public double M11, M12, M13, M14;
        public double M21, M22, M23, M24;
        public double M31, M32, M33, M34;
        public double M41, M42, M43, M44;


        public Matrix4x4(
            double m11, double m12, double m13, double m14, 
            double m21, double m22, double m23, double m24, 
            double m31, double m32, double m33, double m34, 
            double m41, double m42, double m43, double m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;

            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;

            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;

            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public static Matrix4x4 CreateIdentity()
        {
            return new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
                );
        }

        public static Matrix4x4 CreateTranslation(Vector3D position)
        {
            Matrix4x4 result;

            result.M11 = 1.0f;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;

            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = 0.0f;

            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = 0.0f;

            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1.0f;

            return result;
        }

        public static Matrix4x4 CreateScale(Vector3D scale)
        {
            Matrix4x4 result;

            result.M11 = scale.X;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;

            result.M21 = 0.0f;
            result.M22 = scale.Y;
            result.M23 = 0.0f;
            result.M24 = 0.0f;

            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = scale.Z;
            result.M34 = 0.0f;

            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;

            return result;
        }

        public static Matrix4x4 CreateScale(Vector3D scale, Vector3D center)
        {
            Vector3D t = center.Hadamard(Vector3D.One - scale);
            return CreateScale(t);
        }

        public static Matrix4x4 CreateLookAt(Vector3D eye, Vector3D center, Vector3D up)
        {
            up = up.Normalize();    // direction vector -> normalize

            Vector3D forward = (center - eye).Normalize();  // forward vector
            Vector3D left = up.Cross(forward).Normalize();  // orthogonal vector
            Vector3D up2 = forward.Cross(left).Normalize(); // force orthogonality for up vector, incase up and forward were not orthogonal

            return new Matrix4x4(
                left.X,      left.Y,      left.Z,       left.Dot(-eye),
                up2.X,       up2.Y,       up2.Z,        up2.Dot(-eye),
                -forward.X,  -forward.Y,  -forward.Z,   -forward.Dot(-eye),
                0,           0,           0,            1
                );
        }

        public static Vector3D operator *(Matrix4x4 mat, Vector3D vec)
        {
            double a = vec.X * mat.M11 + vec.Y * mat.M12 + vec.Z * mat.M13 + mat.M14;
            double b = vec.X * mat.M21 + vec.Y * mat.M22 + vec.Z * mat.M23 + mat.M24;
            double c = vec.X * mat.M31 + vec.Y * mat.M32 + vec.Z * mat.M33 + mat.M34;
            double w = vec.X * mat.M41 + vec.Y * mat.M42 + vec.Z * mat.M43 + mat.M44;

            return new Vector3D(a / w, b / w, c / w);
        }

        /*
        public static Vector3D operator *(Vector3D vec, Matrix4x4 mat)
        {
            double a = vec.X * mat.M11 + vec.Y * mat.M21 + vec.Z * mat.M31 + mat.M41;
            double b = vec.X * mat.M12 + vec.Y * mat.M22 + vec.Z * mat.M32 + mat.M42;
            double c = vec.X * mat.M13 + vec.Y * mat.M23 + vec.Z * mat.M33 + mat.M43;
            double w = vec.X * mat.M14 + vec.Y * mat.M24 + vec.Z * mat.M34 + mat.M44;

            return new Vector3D(a / w, b / w, c / w);
        }
        */
        
        public bool Equals(Matrix4x4 other)
        {
            return M11.Equals(other.M11) && M12.Equals(other.M12) && M13.Equals(other.M13) && M14.Equals(other.M14) && M21.Equals(other.M21) && M22.Equals(other.M22) && M23.Equals(other.M23) && M24.Equals(other.M24) && M31.Equals(other.M31) && M32.Equals(other.M32) && M33.Equals(other.M33) && M34.Equals(other.M34) && M41.Equals(other.M41) && M42.Equals(other.M42) && M43.Equals(other.M43) && M44.Equals(other.M44);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Matrix4x4 && Equals((Matrix4x4) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = M11.GetHashCode();
                hashCode = (hashCode * 397) ^ M12.GetHashCode();
                hashCode = (hashCode * 397) ^ M13.GetHashCode();
                hashCode = (hashCode * 397) ^ M14.GetHashCode();

                hashCode = (hashCode * 397) ^ M21.GetHashCode();
                hashCode = (hashCode * 397) ^ M22.GetHashCode();
                hashCode = (hashCode * 397) ^ M23.GetHashCode();
                hashCode = (hashCode * 397) ^ M24.GetHashCode();

                hashCode = (hashCode * 397) ^ M31.GetHashCode();
                hashCode = (hashCode * 397) ^ M32.GetHashCode();
                hashCode = (hashCode * 397) ^ M33.GetHashCode();
                hashCode = (hashCode * 397) ^ M34.GetHashCode();

                hashCode = (hashCode * 397) ^ M41.GetHashCode();
                hashCode = (hashCode * 397) ^ M42.GetHashCode();
                hashCode = (hashCode * 397) ^ M43.GetHashCode();
                hashCode = (hashCode * 397) ^ M44.GetHashCode();
                return hashCode;
            }
        }
    }
}