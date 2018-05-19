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
            var viewPos = new Vector3D(0, 0, 0);
            var viewDir = Vector3D.Forward;

            var camera = new Camera();
            camera.RecalculateMatrix(viewPos, viewPos + viewDir);

            var scene = new Scene(camera);

            scene.Renderables.Add(new Sphere(new Vector3D(0, 0, 0),
                new Material(Material.MaterialType.Lambert, new Vector3D(1, 0, 0), new Vector3D(0, 0, 0), 0.2, 0), 1));

            return scene;
        }
    }
}