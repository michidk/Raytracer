using Raytracer.Types;

namespace Raytracer
{
    public class Material
    {
        public enum MaterialType
        {
            Lambert,
            Metal,
            Dialectric
        }

        public Vector3D Albedo;
        public Vector3D Emissive;
        public double Ri;
        public double Roughness;

        public MaterialType Type;

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