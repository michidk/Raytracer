using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raytracer.CustomMath;

namespace Raytracer
{
    public class Scene
    {
        public Camera Camera;
        public List<Renderable> Renderables = new List<Renderable>();

        public Scene(Camera camera)
        {
            Camera = camera;
        }

        public static Scene BuildExampleScene()
        {
            var scene = new Scene(new Camera(new Vector3D(0, 2, 3), Vector3D.ZERO, Vector3D.UP));
            scene.Renderables.Add(new Sphere(new Vector3D(1, 1, 1), new Material(Material.MaterialType.Lambert, new Vector3D(1f, 0f, 0f), new Vector3D(0f,0f,0f), 0.2f, 0f ), 4f));

            return scene;
        }
    }
}
