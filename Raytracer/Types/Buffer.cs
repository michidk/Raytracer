using System;

namespace Raytracer.Types
{
    public class Buffer
    {
        public Size2D Size;

        public Color[,] RawData => data;
        private Color[,] data;

        public Buffer(Size2D size)
        {
            Size = size;
            data = new Color[size.Width, size.Height];
        }

        public void SetPixel(int x, int y, Color color)
        {
            data[x, y] = color;
        }

        public void SetPixel(Point2D pixel, Color color)
        {
            SetPixel(pixel.X, pixel.Y, color);
        }

        public Color GetPixel(int x, int y)
        {
            return data[x, y];
        }

        public Color GetPixel(Point2D pixel)
        {
            return GetPixel(pixel.X, pixel.Y);
        }

        public void Clear(Color color)
        {
            for (var x = 0; x < Size.Width; x++)
            for (var y = 0; y < Size.Height; y++)
            {
                data[x, y] = color;
            }
        }

        public void CopyTo(ref Buffer newBuffer)
        {
            var newSize = newBuffer.Size;
            for (var x = 0; x < Math.Min(Size.Width, newSize.Width); x++)
            for (var y = 0; y < Math.Min(Size.Height, newSize.Height); y++)
            {
                newBuffer.data[x, y] = data[x, y];
            }
        }

        public void Resize(Size2D newSize)
        {
            if (Size == newSize)
                return;

            var newData = new Color[newSize.Width, newSize.Height];

            for (var x = 0; x < newSize.Width; x++)
            for (var y = 0; y < newSize.Height; y++)
            {
                if (x < Size.Width && y < Size.Height)
                {
                    newData[x, y] = data[x, y];
                }
            }

            data = newData;
            Size = newSize;
        }
    }
}