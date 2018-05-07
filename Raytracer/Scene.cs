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
        public Camera Camera { get; private set; }
        public List<Renderable> Renderables { get; private set;}

        public Scene(Camera camera)
        {
            Camera = camera;
            Renderables = new List<Renderable>();
        }

        public static Scene BuildExampleScene()
        {
            var scene = new Scene(new Camera(new Vector3D(1, 0, -10), new Vector3D(-20, 0, 0), Vector3D.UP));
            scene.Renderables.Add(new Sphere(new Vector3D(0, 0, 0), new Material(Material.MaterialType.Lambert, new Vector3D(1, 0, 0), new Vector3D(0, 0, 0), 0.2, 0), 1));

            return scene;
        }
    }
}
