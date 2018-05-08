using Raytracer.Types;

namespace Raytracer
{
    public abstract class Renderable
    {
        public Material Material;
        public Vector3D Position;

        protected Renderable(Vector3D position, Material material)
        {
            Position = position;
            Material = material;
        }

        public abstract RaycastHit HitObject(Ray ray, double nearPlane, double farPlane);
    }
}