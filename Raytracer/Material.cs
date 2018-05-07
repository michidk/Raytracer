using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raytracer.CustomMath;

namespace Raytracer
{
    public class Material
    {
        public enum MaterialType { Lambert, Metal, Dialectric }

        public MaterialType Type;
        public Vector3D Albedo;
        public Vector3D Emissive;
        public double Roughness;
        public double Ri;

        public Material(MaterialType type, Vector3D albedo, Vector3D emissive, double roughness, double ri)
        {
            Type = type;
            Albedo = albedo;
            Emissive = emissive;
            Roughness = roughness;
            Ri = ri;
        }
    }
}
