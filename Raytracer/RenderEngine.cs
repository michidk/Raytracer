using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Raytracer.CustomMath;

namespace Raytracer
{
    public class RenderEngine
    {

        private Buffer buffer = new Buffer(new Size2D(0, 0));


        public BitmapSource Render(Size2D size)
        {
            //buffer.Clear(Color.FromSysColor(System.Drawing.Color.AliceBlue));
            buffer.Resize(size);

            for (int x = 0; x < size.Width; x++)
            {
                for (int y = 0; y < size.Height; y++)
                {
                    var pos = new Point2D(x, y);

                    // clear color
                    Color color = Color.FromSysColor(System.Drawing.Color.AliceBlue);
                    float samplesPerPixel = 1;
                    for (int i = 0; i < 5; i++)
                    {
                        Color sample = Color.FromSysColor(System.Drawing.Color.Red);
                        // trace ray
                        color += sample / samplesPerPixel;
                    }

                    //gamma correct
                    // float3(sqrtf(col.x), sqrtf(col.y), sqrtf(col.z));
                    color.Red = (byte) System.Math.Sqrt(color.Red);
                    color.Green = (byte) System.Math.Sqrt(color.Green);
                    color.Blue = (byte) System.Math.Sqrt(color.Blue);
                    buffer.SetPixel(pos, color);
                    //buffer.SetPixel(pos, new Color((byte) (x % 255), (byte) (y % 255), (byte) (x % 255)));

                }

            }

            return Utils.GetBitmapSourceFromArray(buffer.RawData, size);
        }

        private Color Trace()
        {
            return new Color(0x0b, 0x0b, 0x0b);
        }

        private bool HitWorld(Scene scene)
        {
            foreach (var renderable in scene)
            {
                renderable.
            }

            return false;
        }

    }
}
