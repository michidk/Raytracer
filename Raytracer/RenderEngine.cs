using System;
using System.Windows.Media.Imaging;
using Raytracer.Types;
using Raytracer.Utils;
using Buffer = Raytracer.Types.Buffer;
using Color = System.Drawing.Color;

namespace Raytracer
{
    public class RenderEngine
    {
        private const int SAMPLES_PER_PIXEL = 1;

        private readonly Buffer buffer = new Buffer(Size2D.Zero);
        private readonly Random random = new Random();

        public RenderEngine(Scene scene)
        {
            if (scene.Camera == null) throw new ArgumentException("Camera missing in Scene!");

            Scene = scene;
        }

        public Scene Scene { get; }

        public BitmapSource Render(Size2D size)
        {
            //buffer.Clear(Color.FromSysColor(System.Drawing.Color.AliceBlue));
            buffer.Resize(size);
            Scene.Camera.UpdateViewport(size);

            //double invWidth = 1.0f / size.Width;
            //double invHeight = 1.0f / size.Height;

            for (var x = 0; x < size.Width; x++)
            for (var y = 0; y < size.Height; y++)
            {
                var pos = new Point2D(x, y);

                // clear color
                var color = HPColor.FromSysColor(Color.Black);

                for (var i = 0; i < SAMPLES_PER_PIXEL; i++)
                {
                    //var u = (x + random.NextDouble()) * invWidth;
                    //var v = (y + random.NextDouble()) * invHeight;

                    // trace ray
                    var ray = Scene.Camera.GetRay(x, y); //TODO: use random
                    var col = Trace(ray);
                    color += col / SAMPLES_PER_PIXEL;
                }

                // correct gamma and convert to bytes
                //color.Red = (byte) Math.Sqrt(color.Red);
                //color.Green = (byte) Math.Sqrt(color.Green);
                //color.Blue = (byte) Math.Sqrt(color.Blue);

                // lerp with previous color
                //var prevCol = buffer.GetPixel(pos);
                //color = prevCol * lerpFactor + color * (1 - lerpFactor);

                buffer.SetPixel(pos, Types.Color.FromHPColor(color));
                //buffer.SetPixel(pos, new Color((byte) (x % 255), (byte) (y % 255), (byte) (x % 255)));
            }

            return BitmapUtils.GetBitmapSourceFromArray(buffer.RawData, size);
        }

        private HPColor Trace(Ray ray)
        {
            var hit = HitWorld(ray);

            if (hit.Hit) // render object
            {
                var l = -new Vector3D(-1, -1, 1).Normalize();
                double product = l.Dot(hit.Normal);
                product = Math.Min(Math.Max(product, 0), 1);
                return HPColor.FromSysColor(Color.Green) * product;
            }

            //return HPColor.FromSysColor(System.Drawing.Color.Red);
            var r = ray.Direction;
            var t = 0.5 * (r.Y + 1);
            return (new HPColor(1, 1, 1) * (1 - t) + new HPColor(0.5, 0.5, 1) * t) * 0.3;
            return HPColor.FromSysColor(Color.Aqua);
        }

        private RaycastHit HitWorld(Ray ray)
        {
            var cam = Scene.Camera;

            var closestHit = new RaycastHit
            {
                Hit = false,
                DistanceToHit = cam.MaxPlane,
            };

            foreach (var renderable in Scene.Renderables)
            {
                var hit = renderable.HitObject(ray, cam.MinPlane, closestHit.DistanceToHit);

                if (hit.Hit && hit.DistanceToHit <= closestHit.DistanceToHit)  // second check not really necessary because we already pass it as new far plane to HitObject(..)
                {
                    closestHit = hit;
                }
            }

            return closestHit;
        }
    }
}