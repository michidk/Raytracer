using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Raytracer.CustomMath;

namespace Raytracer
{
    public class RenderEngine
    {

        private const int SAMPLES_PER_PIXEL = 1;

        private Scene scene;

        private Random random = new Random();
        private Buffer buffer = new Buffer(new Size2D(0, 0));

        public RenderEngine(Scene scene)
        {
            this.scene = scene;
        }

        public BitmapSource Render(Size2D size)
        {
            //buffer.Clear(Color.FromSysColor(System.Drawing.Color.AliceBlue));
            buffer.Resize(size);

            double invWidth = 1.0f / size.Width;
            double invHeight = 1.0f / size.Height;

            for (int x = 0; x < size.Width; x++)
            {
                for (int y = 0; y < size.Height; y++)
                {
                    var pos = new Point2D(x, y);

                    // clear color
                    Color color = Color.FromSysColor(System.Drawing.Color.AliceBlue);
                    
                    for (int i = 0; i < SAMPLES_PER_PIXEL; i++)
                    {
                        double u = (x + random.NextDouble()) * invWidth;
                        double v = (y + random.NextDouble()) * invHeight;

                        // trace ray
                        Ray ray = scene.Camera.GetRay(random, u, v);
                        color += Trace(ray) / SAMPLES_PER_PIXEL;
                    }

                    // correct gamma and convert to bytes
                    //color.Red = (byte) Math.Sqrt(color.Red);
                    //color.Green = (byte) Math.Sqrt(color.Green);
                    //color.Blue = (byte) Math.Sqrt(color.Blue);

                    // lerp with previous color
                    //var prevCol = buffer.GetPixel(pos);
                    //color = prevCol * lerpFactor + color * (1 - lerpFactor);

                    buffer.SetPixel(pos, color);
                    //buffer.SetPixel(pos, new Color((byte) (x % 255), (byte) (y % 255), (byte) (x % 255)));
                }

            }

            return Utils.GetBitmapSourceFromArray(buffer.RawData, size);
        }

        private Color Trace(Ray ray)
        {
            RaycastHit hit = HitWorld(ray);

            if (hit.Hit)    // render object
            {
                return Color.FromSysColor(System.Drawing.Color.Green);
            }
            else            // render sky
            {
                return Color.FromSysColor(System.Drawing.Color.Red);
            }
        }

        private RaycastHit HitWorld(Ray ray)
        {
            var cam = scene.Camera;

            RaycastHit lastHit = new RaycastHit {Hit = false};
            double closest = cam.MaxPlane;

            foreach (var renderable in scene.Renderables)
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
