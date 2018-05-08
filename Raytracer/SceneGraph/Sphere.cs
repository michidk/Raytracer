using System;
using Raytracer.Types;

namespace Raytracer
{
    public class Sphere : Renderable
    {
        public double Radius;

        public Sphere(Vector3D position, Material material, double radius) : base(position, material)
        {
            Radius = radius;
        }

        // reference: https://www.cs.princeton.edu/courses/archive/fall00/cs426/lectures/raycast/raycast.pdf
        // https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection
        public override RaycastHit HitObject(Ray ray, double nearPlane, double farPlane)
        {
            var dist = Position - ray.Origin;                   // distance between sphere center and ray origin
            var proj = dist.Dot(ray.Direction);                 // length of (project distance vector onto direction) 

            if (proj < 0)
                return new RaycastHit();                        // projected vector points to opposite direction and therefor can't hit the sphere

            var squaredD = dist.Dot(dist) - Math.Pow(proj, 2);  // squared length from sphere center to hit point on raycast direction (orthogonal to raycast dir)
            var squaredRadius = Math.Pow(Radius, 2);            // squared radius to compare with d

            if (squaredD > squaredRadius)                       // hit point on raycast direction outside radius
                return new RaycastHit();

            var innerT = Math.Sqrt(squaredRadius - squaredD);   // distance from hit point to center projected on raycast dir

            var t1 = proj - innerT;                             // distance from ray origin to first hit on sphere
            var t2 = proj + innerT;                             // distance from ray origin to second hit on sphere
            var t1Hit = t1 > nearPlane && t1 < farPlane;
            var t2Hit = t2 > nearPlane && t2 < farPlane;

            if (t1Hit || t2Hit)
            {
                var t = t1Hit ? t1 : t2;

                var hit = new RaycastHit
                {
                    Hit = true,
                    DistanceToHit = t,
                    Position = ray.PointAt(t)
                };

                return hit;
            }

            return new RaycastHit();
        }
    }
}