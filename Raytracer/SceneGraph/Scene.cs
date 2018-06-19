using System.Collections.Generic;
using Raytracer.Types;

namespace Raytracer
{
    public class Scene
    {
        public Scene(Camera camera)
        {
            Camera = camera;
            Renderables = new List<Renderable>();
        }

        public Camera Camera { get; }
        public List<Renderable> Renderables { get; }

        public static Scene BuildExampleScene()
        {
            var camera = new Camera(new Vector3D(0, 0, -5), Vector3D.Forward);

            var scene = new Scene(camera);

            scene.Renderables.Add(new Sphere(new Vector3D(1, 1, 1), new Material(Material.MaterialType.Lambert, Color.FromSysColor(System.Drawing.Color.Aquamarine), new Color(0, 0, 0), 0.2, 0), 1));
            scene.Renderables.Add(new Sphere(new Vector3D(0, 1, 0), new Material(Material.MaterialType.Lambert, Color.FromSysColor(System.Drawing.Color.Coral), new Color(0, 0, 0), 0.2, 0), 1));
            scene.Renderables.Add(new Sphere(new Vector3D(-1, -1, 4), new Material(Material.MaterialType.Lambert, Color.FromSysColor(System.Drawing.Color.Chartreuse), Color.FromSysColor(System.Drawing.Color.Chartreuse), 0.2, 0), 4));

            return scene;
        }
    }
}