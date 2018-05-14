using System;
using System.Configuration;
using Raytracer.Extensions;
using Raytracer.Types;
using Raytracer.Utils;

namespace Raytracer
{
    public class Camera
    {
        public Camera(Vector3D position, Vector3D direction, double verticalFov = 60.0, float minPlane = 0.001f, float maxPlane = 10e7f, double aperture = 0.1)
        {
            SetPositionAndDirection(position, direction);

            VerticalFov = verticalFov;
            MinPlane = minPlane;
            MaxPlane = maxPlane;
            Aperture = aperture;
        }

        #region Positon, Direction and View-Matrix
        private Vector3D position;
        public Vector3D Position
        {
            get => position;
            set
            {
                position = value;
                CameraToWorldMatrix = CalculateCameraToWorldMatrix(position, direction);
            }
        }

        private Vector3D direction;
        public Vector3D Direction
        {
            get => direction;
            set
            {
                direction = value;
                CameraToWorldMatrix = CalculateCameraToWorldMatrix(position, direction);
            }
        }

        public Matrix4x4 CameraToWorldMatrix { get; private set; }
        #endregion

        #region Fov and Scale
        private double verticalFov;
        public double VerticalFov
        {
            get => verticalFov;
            set
            {
                verticalFov = value;
                Scale = CalculateScaleFromVerticalFov(verticalFov);
            }
        }
        
        public double Scale { get; private set; }
        #endregion

        #region ScreenSize and Aspect Ratio

        private Size2D screenSize;

        public Size2D ScreenSize
        {
            get => screenSize;
            set
            {
                screenSize = value;
                AspectRatio = CalculateAspectRationFromScreenSize(screenSize);
            }
        }

        public double AspectRatio { get; private set; }
        #endregion

        #region Frustum
        public double MinPlane { get; set; }
        public double MaxPlane { get; set; }
        #endregion

        #region Aperture and Lens Radius
        private double aperture;

        public double Aperture
        {
            get => aperture;
            set
            {
                aperture = value;
                lensRadius = aperture / 2.0;
            }
        }

        private double lensRadius;

        public double LensRadius
        {
            get => lensRadius;
            set
            {
                lensRadius = value;
                aperture = lensRadius * 2.0;
            }
        }
        #endregion

        public void SetPositionAndDirection(Vector3D position, Vector3D direction)
        {
            this.position = position;
            this.direction = direction.Normalize();
            this.CameraToWorldMatrix = CalculateCameraToWorldMatrix(this.position, this.direction);
        }

        public Ray GetRay(double x, double y)
        {
            double u = (2 * (x + 0.5) / (double) ScreenSize.Width - 1) * AspectRatio * Scale;
            double v = (1 - 2 * (y + 0.5) / (double) ScreenSize.Height) * Scale;

            var dir = CameraToWorldMatrix.MultiplyDirection(new Vector3D(u, v, 1)).Normalize();

            return new Ray(position, dir);
        }

        public Ray GetRandomizedRay(double x, double y, Random random)
        {
            double u = (2 * (x + random.NextDouble()) / (double) ScreenSize.Width - 1) * AspectRatio * Scale;
            double v = (1 - 2 * (y + random.NextDouble()) / (double) ScreenSize.Height) * Scale;

            Vector3D dir = CameraToWorldMatrix.MultiplyDirection(new Vector3D(u, v, 1)).Normalize();

            Vector3D rd = lensRadius * random.RandomInsideUnitDisk();
            Vector3D offset = CameraToWorldMatrix.MultiplyDirection(Vector3D.Up) * rd.X + CameraToWorldMatrix.MultiplyDirection(Vector3D.Right) * rd.Y; // ad rd.x in right direction and rd.y in up direction

            return new Ray(position + offset, dir);
        }

        private static double CalculateAspectRationFromScreenSize(Size2D screenSize)
        {
            return screenSize.Width / (double)screenSize.Height;
        }

        private static double CalculateScaleFromVerticalFov(double fov)
        {
            return Math.Tan(DoubleUtils.DegreesToRadians(fov * 0.5));
        }

        private static Matrix4x4 CalculateCameraToWorldMatrix(Vector3D position, Vector3D direction)
        {
            return Matrix4x4.CreateTRS(position, direction, Vector3D.One);
        }

    }
}