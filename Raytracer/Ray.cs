using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raytracer.CustomMath;

namespace Raytracer
{
    public class Ray
    {
        public Vector3D Origin;
        public Vector3D Direction;
        public double NearPlane;
        public double FarPlane;

        public Vector3D PointAt(double distance)
        {
            return Origin + Direction * distance;
        }
    }
}
