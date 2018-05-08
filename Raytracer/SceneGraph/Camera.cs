using System;
using Raytracer.Types;
using Raytracer.Utils;

namespace Raytracer
{
    public class Camera
    {
        public Camera(Vector3D position, Vector3D lookAt, Vector3D upwards, double verticalFov = 60.0,
            double aperture = 0.1d, double focusDistance = 3.0, float minPlane = 0.001f, float maxPlane = 10e7f,
            float maxDepth = 10f)
        {
            Position = position;
            LookAt = lookAt;
            Upwards = upwards;
            VerticalFov = verticalFov;
            Aperture = aperture;
            FocusDistance = focusDistance;
            MinPlane = minPlane;
            MaxPlane = maxPlane;
            MaxDepth = maxDepth;

            CalculateLensRadius(focusDistance);
            //CalculateCameraPropertiesFromConstructor(lookAt, upwards); done via Resize
        }

        public Vector3D Position { get; private set; }
        public Vector3D LookAt { get; }
        public Vector3D Upwards { get; }

        public double VerticalFov { get; }
        public double AspectRatio { get; private set; }
        public double Aperture { get; }
        public double FocusDistance { get; }

        public double MinPlane { get; }
        public double MaxPlane { get; }
        public double MaxDepth { get; }

        public double LensRadius { get; private set; }

        // transformed vectors: u horizontally, v is pointing upwards from camera matrix, w is the camera directionv
        public Vector3D U { get; private set; }
        public Vector3D V { get; private set; } // TODO: really necessary? or just store direction and positon?
        public Vector3D W { get; private set; }

        public Vector3D LowerLeftCorner { get; private set; }
        public Vector3D Horizontal { get; private set; }
        public Vector3D Vertical { get; private set; }

        public void UpdateViewport(double aspectRatio)
        {
            if (Math.Abs(AspectRatio - aspectRatio) > 0.00001) // recalculate only if necessary
            {
                AspectRatio = aspectRatio;
                CalculateCameraPropertiesFromConstructor(LookAt, Upwards);
            }
        }

        public void UpdatePosition(Vector3D newPos)
        {
            if (Position != newPos)
            {
                Position = newPos;
                CalculateCameraPropertiesFromConstructor(LookAt, Upwards);
            }
        }

        public Ray GetRay(double horizontal, double vertical)
        {
            if (AspectRatio == 0)
                throw new ArgumentException("Camera was not initialized or aspect-ratio was set to 0.");

            var offset = U * LensRadius + V * LensRadius;
            var origin = Position + offset;
            var dir = LowerLeftCorner + horizontal * Horizontal + vertical * Vertical - origin;
            return new Ray(origin, dir.Normalize());
        }

        public Ray GetRayRandomized(Random random, double horizontal, double vertical)
        {
            if (AspectRatio == 0)
                throw new ArgumentException("Camera was not initialized or aspect-ratio was set to 0.");

            var rand = LensRadius * random.RandomInsideUnitDisk();
            var offset = U * rand.X + V * rand.Y;
            var origin = Position + offset;
            var dir = LowerLeftCorner + horizontal * Horizontal + vertical * Vertical - origin;
            return new Ray(origin, dir.Normalize());
        }

        public void CalculateLensRadius(double focusDistance)
        {
            LensRadius = focusDistance / 2.0;
        }

        public void CalculateCameraPropertiesFromConstructor(Vector3D dir, Vector3D up)
        {
            var theta = VerticalFov * Math.PI / 180.0;
            var halfHeight = Math.Tan(theta / 2.0);
            var halfWidth = AspectRatio * halfHeight;

            W = (Position - dir).Normalize();
            U = up.Cross(W).Normalize();
            V = W.Cross(U); // no need to normalize, since crossing two normalized vectors results in a normalized vector

            var halfHorz = U * halfWidth * FocusDistance;
            var halfVert = V * halfHeight * FocusDistance;
            Horizontal = halfHorz * 2;
            Vertical = halfVert * 2;
            LowerLeftCorner = Position - halfHorz - halfVert - W * FocusDistance;
        }
    }
}