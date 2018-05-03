using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raytracer.CustomMath;

namespace Raytracer
{
    public struct RaycastHit
    {
        public bool Hit;


        public Vector3D Position;
        public Vector3D Normal;
        public double DistanceToHit;
    }
}
