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
        public Vector3D Position { get; private set; }
        public Vector3D LookAt { get; private set; }
        public Vector3D Upwards { get; private set; }

        public double VerticalFov { get; private set; }
        public double AspectRatio { get; private set; }
        public double Aperture { get; private set; }
        public double FocusDistance { get; private set; }

        public double MinPlane { get; private set; }
        public double MaxPlane { get; private set; }
        public double MaxDepth { get; private set; }

        public double LensRadius { get; private set; }

        // transformed vectors: u horizontally, v is pointing upwards from camera matrix, w is the camera directionv
        public Vector3D U { get; private set; }
        public Vector3D V { get; private set; }     // TODO: really necessary? or just store direction and positon?
        public Vector3D W { get; private set; }   
        
        public Vector3D LowerLeftCorner { get; private set; }
        public Vector3D Horizontal { get; private set; }
        public Vector3D Vertical { get; private set; }


        public Camera(Vector3D position, Vector3D lookAt, Vector3D upwards, double verticalFov = 60.0, double aperture = 0.1d, double focusDistance = 3.0, float minPlane = 0.001f, float maxPlane = 10e7f, float maxDepth = 10f)
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

        public void Resize(double aspectRatio)
        {
            if (Math.Abs(AspectRatio - aspectRatio) > 0.00001)  // recalculate only if necessary
            {
                AspectRatio = aspectRatio;
                CalculateCameraPropertiesFromConstructor(LookAt, Upwards);
            }
        }

        public Ray GetRay(Random random, double horizontal, double vertical)
        {
            if (AspectRatio == 0)
            {
                throw new ArgumentException("Camera was not initialized or aspect-ratio was set to 0.");
            }

            Vector3D rand = LensRadius * random.RandomInsideUnitDisk();
            Vector3D offset = U * rand.X + V * rand.Y;
            Vector3D origin = Position + offset;
            Vector3D dir = LowerLeftCorner + horizontal * Horizontal + vertical * Vertical - origin;
            return new Ray(origin, dir.Normalize());
        }
        public Ray GetRayNotRandom(double horizontal, double vertical)
        {
            if (AspectRatio == 0)
            {
                throw new ArgumentException("Camera was not initialized or aspect-ratio was set to 0.");
            }

            Vector3D offset = U * LensRadius + V * LensRadius;
            Vector3D origin = Position + offset;
            Vector3D dir = LowerLeftCorner + horizontal * Horizontal + vertical * Vertical - origin;
            return new Ray(origin, dir.Normalize());
        }

        public void CalculateLensRadius(double focusDistance)
        {
            LensRadius = focusDistance / 2.0;
        }

        public void CalculateCameraPropertiesFromConstructor(Vector3D dir, Vector3D up)
        {
            double theta = VerticalFov * Math.PI / 180.0;
            double halfHeight = Math.Tan(theta / 2.0);
            double halfWidth = AspectRatio * halfHeight;

            W = (Position - dir).Normalize();
            U = up.Cross(W).Normalize();
            V = W.Cross(U); // no need to normalize, since crossing two normalized vectors results in a normalized vector

            Vector3D halfHorz = U * halfWidth * FocusDistance;
            Vector3D halfVert = V * halfHeight * FocusDistance;
            Horizontal = halfHorz * 2;
            Vertical = halfVert * 2;
            LowerLeftCorner = Position - halfHorz - halfVert - W * FocusDistance;
        }

    }
}
