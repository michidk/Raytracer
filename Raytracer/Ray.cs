using Raytracer.Types;

namespace Raytracer
{
    public class Ray
    {
        public Vector3D Direction;
        public Vector3D Origin;

        public Ray(Vector3D origin, Vector3D direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3D PointAt(double distance)
        {
            return Origin + Direction * distance;
        }
    }
}