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
            result.M14 = position.X;

            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = position.Y;

            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = position.Z;

            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
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

        public static Matrix4x4 CreateLookAt(Vector3D eye, Vector3D target, Vector3D up)
        {
            up = up.Normalize();    // direction vector -> normalize

            Vector3D forward = (target - eye).Normalize();  // forward vector
            Vector3D right = up.Cross(forward).Normalize();  // orthogonal vector
            Vector3D up2 = forward.Cross(right).Normalize(); // force orthogonality for up vector, incase up and forward were not orthogonal

            /*return new Matrix4x4(
                right.X,      right.Y,      right.Z,       right.Dot(-eye),
                up2.X,       up2.Y,       up2.Z,        up2.Dot(-eye),
                -forward.X,  -forward.Y,  -forward.Z,   -forward.Dot(-eye),
                0,           0,           0,            1
                );*/
            var rot = new Matrix4x4(
                right.X, up2.X, forward.X, 0,
                right.Y, up2.Y, forward.Y, 0,
                right.Z, up2.Z, forward.Z, 0,
                0, 0, 0, 1
            );

            return rot * CreateTranslation(eye);
        }

        public static Matrix4x4 operator *(Matrix4x4 l, Matrix4x4 r)
        {
            Matrix4x4 res;

            // Row 1
            res.M11 = l.M11 * r.M11 + l.M12 * r.M21 + l.M13 * r.M31 + l.M14 * r.M41;  // Column 1
            res.M12 = l.M11 * r.M12 + l.M12 * r.M22 + l.M13 * r.M32 + l.M14 * r.M42;  // Column 2
            res.M13 = l.M11 * r.M13 + l.M12 * r.M23 + l.M13 * r.M33 + l.M14 * r.M43;  // Column 3
            res.M14 = l.M11 * r.M14 + l.M12 * r.M24 + l.M13 * r.M34 + l.M14 * r.M44;  // Column 4

            // Row 2
            res.M21 = l.M21 * r.M11 + l.M22 * r.M21 + l.M23 * r.M31 + l.M24 * r.M41;  // Column 1
            res.M22 = l.M21 * r.M12 + l.M22 * r.M22 + l.M23 * r.M32 + l.M24 * r.M42;  // Column 2
            res.M23 = l.M21 * r.M13 + l.M22 * r.M23 + l.M23 * r.M33 + l.M24 * r.M43;  // Column 3
            res.M24 = l.M21 * r.M14 + l.M22 * r.M24 + l.M23 * r.M34 + l.M24 * r.M44;  // Column 4

            // Row 3
            res.M31 = l.M31 * r.M11 + l.M32 * r.M21 + l.M33 * r.M31 + l.M34 * r.M41;  // Column 1
            res.M32 = l.M31 * r.M12 + l.M32 * r.M22 + l.M33 * r.M32 + l.M34 * r.M42;  // Column 2
            res.M33 = l.M31 * r.M13 + l.M32 * r.M23 + l.M33 * r.M33 + l.M34 * r.M43;  // Column 3
            res.M34 = l.M31 * r.M14 + l.M32 * r.M24 + l.M33 * r.M34 + l.M34 * r.M44;  // Column 4

            // Row 4
            res.M41 = l.M41 * r.M11 + l.M42 * r.M21 + l.M43 * r.M31 + l.M44 * r.M41;  // Column 1
            res.M42 = l.M41 * r.M12 + l.M42 * r.M22 + l.M43 * r.M32 + l.M44 * r.M42;  // Column 2
            res.M43 = l.M41 * r.M13 + l.M42 * r.M23 + l.M43 * r.M33 + l.M44 * r.M43;  // Column 3
            res.M44 = l.M41 * r.M14 + l.M42 * r.M24 + l.M43 * r.M34 + l.M44 * r.M44;  // Column 4

            return res;
        }

        //public static Vector3D operator *(Matrix4x4 mat, Vector3D vec)
        public Vector3D MultiplyPosition(Vector3D vec)
        {
            double a = vec.X * M11 + vec.Y * M12 + vec.Z * M13 + M14;
            double b = vec.X * M21 + vec.Y * M22 + vec.Z * M23 + M24;
            double c = vec.X * M31 + vec.Y * M32 + vec.Z * M33 + M34;
            //double w = vec.X * mat.M41 + vec.Y * mat.M42 + vec.Z * mat.M43 + mat.M44;

            //return new Vector3D(a / w, b / w, c / w);
            return new Vector3D(a, b, c);
        }

        public Vector3D MultiplyDirection(Vector3D vec)
        {
            double a = vec.X * M11 + vec.Y * M12 + vec.Z * M13;
            double b = vec.X * M21 + vec.Y * M22 + vec.Z * M23;
            double c = vec.X * M31 + vec.Y * M32 + vec.Z * M33;
            //double w = vec.X * mat.M41 + vec.Y * mat.M42 + vec.Z * mat.M43 + mat.M44;

            //return new Vector3D(a / w, b / w, c / w);
            return new Vector3D(a, b, c);
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