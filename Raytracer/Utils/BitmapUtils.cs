using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Raytracer.Types;

namespace Raytracer.Utils
{
    public static class BitmapUtils
    {

        public static BitmapSource GetBitmapSourceFromBitmap(Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
        }

        public static BitmapSource GetBitmapSourceFromArray(byte[] data, Size2D size)
        {
            var format = PixelFormats.Rgb24;

            var wbm = new WriteableBitmap(size.Width, size.Height, 96, 96, format, null);
            wbm.WritePixels(new Int32Rect(0, 0, size.Width, size.Height), data, format.BitsPerPixel / 8 * size.Width, 0);

            return wbm;
        }

        // creates the most minimal bitmapsource that can be created
        public static BitmapSource CreateEmptyBitmap()
        {
            return BitmapSource.Create(1, 1, 1, 1, PixelFormats.BlackWhite, null, new byte[] {0}, 1);
        }

    }
}