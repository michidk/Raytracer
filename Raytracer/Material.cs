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

        public Color Albedo;
        public Color Emissive;
        public double Ri;
        public double Roughness;

        public MaterialType Type;

        public Material(MaterialType type, Color albedo, Color emissive, double roughness, double ri)
        {
            Type = type;
            Albedo = albedo;
            Emissive = emissive;
            Roughness = roughness;
            Ri = ri;
        }
    }
}