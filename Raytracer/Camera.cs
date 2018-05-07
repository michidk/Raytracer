using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raytracer.CustomMath;

namespace Raytracer
{
    public class Camera
    {
        public Vector3D Position;

        public double VerticalFov;
        public double AspectRatio;
        public double Aperture;
        public double FocusDistance;

        public double MinPlane;
        public double MaxPlane;
        public double MaxDepth;

        public double LensRadius;
        public Vector3D U, V, W;    // transformed vectors: u horizontally, v is pointing upwards from camera matrix, w is the camera directionv
        public Vector3D LowerLeftCorner;
        public Vector3D Horizontal;
        public Vector3D Vertical;


        public Camera(Vector3D position, Vector3D direction, Vector3D upwards, double verticalFov = 60.0, double aspectRatio = 1.0, double aperture = 0.1d, double focusDistance = 3.0, float minPlane = 0.001f, float maxPlane = 10e7f, float maxDepth = 10f)
        {
            Position = position;
            VerticalFov = verticalFov;
            AspectRatio = aspectRatio;
            Aperture = aperture;
            FocusDistance = focusDistance;
            MinPlane = minPlane;
            MaxPlane = maxPlane;
            MaxDepth = maxDepth;

            LensRadius = CalculateLensRadiusFromFocusDistance(focusDistance);
            CalculateCameraPropertiesFromConstructor(this, direction, upwards, out U, out V, out W, out LowerLeftCorner, out Horizontal, out Vertical);
        }

        public Ray GetRay(Random random, double horizontal, double vertical)
        {
            Vector3D rand = LensRadius * random.RandomInsideUnitDisk();
            Vector3D offset = U * rand.X + V * rand.Y;
            Vector3D origin = Position + offset;
            Vector3D dir = LowerLeftCorner + horizontal * Horizontal + vertical * Vertical - origin;
            return new Ray(origin, dir.Normalize());
        }

        public static double CalculateLensRadiusFromFocusDistance(double focusDistance)
        {
            return focusDistance / 2.0;
        }

        public static void CalculateCameraPropertiesFromConstructor(Camera cam, Vector3D dir, Vector3D up, out Vector3D u, out Vector3D v, out Vector3D w, out Vector3D lowerLeftCorner, out Vector3D horizontal, out Vector3D vertical)
        {
            double theta = cam.VerticalFov * Math.PI / 180.0;
            double halfHeight = Math.Tan(theta / 2.0);
            double halfWidth = cam.AspectRatio * halfHeight;

            w = dir.Normalize();
            u = up.Cross(w).Normalize();
            v = w.Cross(u); // no need to normalize, since crossing two normalized vectors results in a normalized vector

            Vector3D halfHorz = u * halfWidth * cam.FocusDistance;
            Vector3D halfVert = v * halfHeight * cam.FocusDistance;
            horizontal = halfHorz * 2;
            vertical = halfVert * 2;
            lowerLeftCorner = cam.Position - halfHorz - halfVert - w * cam.FocusDistance;
        }

    }
}
