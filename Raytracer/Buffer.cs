using System;
using Raytracer.CustomMath;


namespace Raytracer
{
    public class Buffer
    {

        public const int COLOR_STRIDE = 3;

        public Size2D Size;

        private byte[] data;
        private Color lastClearColor;

        public ref byte[] RawData => ref data;


        public Buffer(Size2D size)
        {
            Size = size;

            data = new byte[size.Area * COLOR_STRIDE];
        }

        public int GetIndex(Point2D pixel)
        {
            return (pixel.X + pixel.Y * Size.Width) * COLOR_STRIDE;
        }

        public void SetPixel(Point2D pixel, Color color)
        {
            int idx = GetIndex(pixel);
            data[idx] = color.Red;
            data[++idx] = color.Green;
            data[++idx] = color.Blue;
        }

        public Color GetPixel(Point2D pixel)
        {
            int idx = GetIndex(pixel);
            return new Color(data[idx], data[++idx], data[++idx]);
        }

        public void Clear(Color color)
        {
            for (int x = 0; x < Size.Width; x++)
            {
                for (int y = 0; y < Size.Height; y++)
                {
                    var p = new Point2D(x, y);
                    SetPixel(p, color);
                }
            }

            lastClearColor = color;
        }

        public void CopyTo(ref Buffer newBuffer)
        {
            var newSize = newBuffer.Size;
            for (int x = 0; x < Math.Min(Size.Width, newSize.Width); x++)
            {
                for (int y = 0; y < Math.Min(Size.Height, newSize.Height); y++)
                {
                    var p = new Point2D(x, y);
                    newBuffer.SetPixel(p, GetPixel(p));
                }
            }
        }

        public void Resize(Size2D newSize)
        {
            if (Size == newSize)
                return;

            byte[] newData = new byte[newSize.Area * COLOR_STRIDE];

            for (int x = 0; x < newSize.Width; x++)
            {
                for (int y = 0; y < newSize.Height; y++)
                {
                    int idx = (x + y * newSize.Width) * COLOR_STRIDE;
                    if (x < Size.Width && y < Size.Height)
                    {
                        int oldIdx = (x + y * Size.Width) * COLOR_STRIDE;
                        newData[idx] = data[oldIdx];
                        newData[idx + 1] = data[oldIdx + 1];
                        newData[idx + 2] = data[oldIdx + 2];
                    }
                    else
                    {
                        newData[idx] = lastClearColor.Red;
                        newData[idx + 1] = lastClearColor.Green;
                        newData[idx + 2] = lastClearColor.Blue;
                    }
                }
            }

            this.data = newData;
            this.Size = newSize;
        }

    }
}