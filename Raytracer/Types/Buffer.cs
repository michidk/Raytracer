using System;

namespace Raytracer.Types
{
    public class Buffer
    {
        public const int COLOR_STRIDE = 3;

        private byte[] data;
        private Color lastClearColor;

        public Size2D Size;


        public Buffer(Size2D size)
        {
            Size = size;

            data = new byte[size.Area * COLOR_STRIDE];
        }

        public ref byte[] RawData => ref data;

        public int GetIndex(Point2D pixel)
        {
            return (pixel.X + pixel.Y * Size.Width) * COLOR_STRIDE;
        }

        public void SetPixel(Point2D pixel, Color color)
        {
            var idx = GetIndex(pixel);
            data[idx] = color.Red;
            data[++idx] = color.Green;
            data[++idx] = color.Blue;
        }

        public Color GetPixel(Point2D pixel)
        {
            var idx = GetIndex(pixel);
            return new Color(data[idx], data[++idx], data[++idx]);
        }

        public void Clear(Color color)
        {
            for (var x = 0; x < Size.Width; x++)
            for (var y = 0; y < Size.Height; y++)
            {
                var p = new Point2D(x, y);
                SetPixel(p, color);
            }

            lastClearColor = color;
        }

        public void CopyTo(ref Buffer newBuffer)
        {
            var newSize = newBuffer.Size;
            for (var x = 0; x < Math.Min(Size.Width, newSize.Width); x++)
            for (var y = 0; y < Math.Min(Size.Height, newSize.Height); y++)
            {
                var p = new Point2D(x, y);
                newBuffer.SetPixel(p, GetPixel(p));
            }
        }

        public void Resize(Size2D newSize)
        {
            if (Size == newSize)
                return;

            var newData = new byte[newSize.Area * COLOR_STRIDE];

            for (var x = 0; x < newSize.Width; x++)
            for (var y = 0; y < newSize.Height; y++)
            {
                var idx = (x + y * newSize.Width) * COLOR_STRIDE;
                if (x < Size.Width && y < Size.Height)
                {
                    var oldIdx = (x + y * Size.Width) * COLOR_STRIDE;
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

            data = newData;
            Size = newSize;
        }
    }
}