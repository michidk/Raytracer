using Raytracer.CustomMath;


namespace Raytracer
{
    public abstract class Renderable
    {
        public Vector3D Position;
        public Material Material;

        protected Renderable(Vector3D position, Material material)
        {
            Position = position;
            Material = material;
        }

        public abstract RaycastHit HitObject(Ray ray, double nearPlane, double farPlane);

    }
}
