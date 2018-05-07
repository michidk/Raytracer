using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Raytracer.CustomMath;

namespace Raytracer
{
    public class RenderEngine
    {

        private const int SAMPLES_PER_PIXEL = 1;

        public Scene Scene { get; private set; }

        private Random random = new Random();
        private Buffer buffer = new Buffer(new Size2D(0, 0));

        public RenderEngine(Scene scene)
        {
            if (scene.Camera == null)
            {
                throw new ArgumentException("Camera missing in Scene!");
            }

            this.Scene = scene;
        }

        public BitmapSource Render(Size2D size)
        {
            //buffer.Clear(Color.FromSysColor(System.Drawing.Color.AliceBlue));
            buffer.Resize(size);
            Scene.Camera.Resize((double) size.Width / (double) size.Height);

            double invWidth = 1.0f / size.Width;
            double invHeight = 1.0f / size.Height;

            for (int x = 0; x < size.Width; x++)
            {
                for (int y = 0; y < size.Height; y++)
                {
                    var pos = new Point2D(x, y);

                    // clear color
                    HPColor color = HPColor.FromSysColor(System.Drawing.Color.Black);
                    
                    for (int i = 0; i < SAMPLES_PER_PIXEL; i++)
                    {
                        double u = (x + random.NextDouble()) * invWidth;
                        double v = (y + random.NextDouble()) * invHeight;

                        // trace ray
                        Ray ray = Scene.Camera.GetRayNotRandom(u, v);    //TODO: use random
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

                    buffer.SetPixel(pos, Color.FromHPColor(color));
                    //buffer.SetPixel(pos, new Color((byte) (x % 255), (byte) (y % 255), (byte) (x % 255)));
                }

            }

            return Utils.GetBitmapSourceFromArray(buffer.RawData, size);
        }

        private HPColor Trace(Ray ray)
        {
            RaycastHit hit = HitWorld(ray);

            if (hit.Hit)    // render object
            {
                return HPColor.FromSysColor(System.Drawing.Color.Green);
            }
            else            // render sky
            {
                //return HPColor.FromSysColor(System.Drawing.Color.Red);
                var r = ray.Direction;
                var t = 0.5 * (r.Y + 1);
                return (new HPColor(1, 1, 1) * (1 - t) + new HPColor(0.5, 0.5, 1) * t) * 0.3;
            }
        }

        private RaycastHit HitWorld(Ray ray)
        {
            var cam = Scene.Camera;

            RaycastHit lastHit = new RaycastHit { Hit = false };
            double closest = cam.MaxPlane;

            foreach (var renderable in Scene.Renderables)
            {
                RaycastHit hit = renderable.HitObject(ray, cam.MinPlane, closest);

                if (hit.Hit)
                {
                    lastHit = hit;
                }
            }

            return lastHit;
        }

    }
}
