using System;
using System.Configuration;
using Raytracer.Types;
using Raytracer.Utils;

namespace Raytracer
{
    public class Camera
    {
        public Camera(double verticalFov = 90.0, float minPlane = 0.001f, float maxPlane = 10e7f)
        {
            CameraToWorldMatrix = Matrix4x4.CreateIdentity();
            VerticalFov = verticalFov;
            MinPlane = minPlane;
            MaxPlane = maxPlane;
        }

        public Matrix4x4 CameraToWorldMatrix { get; private set; }

        public double VerticalFov { get; }
        public double AspectRatio { get; private set; }
        public Size2D ScreenSize { get; private set; }

        public double MinPlane { get; }
        public double MaxPlane { get; }

        public Vector3D Position
        {
            get { return CameraToWorldMatrix.MultiplyPosition(Vector3D.Zero); }
        }

        public void UpdateViewport(Size2D screenSize)
        {
            double aspectRatio = screenSize.Width / (double) screenSize.Height;

            if (Math.Abs(AspectRatio - aspectRatio) > 0.00001) // recalculate only if necessary, because this is set every frame
            {
                ScreenSize = screenSize;
                AspectRatio = aspectRatio;
            }
        }

        public void RecalculateMatrix(Vector3D position, Vector3D lookAt)
        {
            CameraToWorldMatrix = Matrix4x4.CreateLookAt(position, lookAt, Vector3D.Up);
        }

        public Ray GetRay(double horizontal, double vertical)
        {
            // --- TODO: move outside and calculate only once
            Vector3D origin = CameraToWorldMatrix.MultiplyPosition(Vector3D.Zero);
            double scale = Math.Tan(DoubleUtils.DegreesToRadians(VerticalFov * 0.5));
            //

            double x = (2 * (horizontal + 0.5) / (double) ScreenSize.Width - 1) * AspectRatio * scale;
            double y = (1 - 2 * (vertical + 0.5) / (double) ScreenSize.Height) * scale;

            var dir = (CameraToWorldMatrix.MultiplyDirection(new Vector3D(x, y, 1))).Normalize();

            return new Ray(origin, dir);
        }

        public Ray GetRayRandomized(Random random, double horizontal, double vertical)
        {
            /* TODO: implement later
            var rand = LensRadius * random.RandomInsideUnitDisk();
            var offset = U * rand.X + V * rand.Y;
            var origin = Position + offset;
            var dir = LowerLeftCorner + horizontal * Horizontal + vertical * Vertical - origin;
            return new Ray(origin, dir.Normalize());
            */
            return new Ray(Vector3D.Zero, Vector3D.Zero);
        }



    }
}