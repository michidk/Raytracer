using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Raytracer.CustomMath;

namespace Raytracer
{
    public static class Utils
    {
        public static BitmapSource GetBitmapSourceFromBitmap(Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
        }

        public static BitmapSource GetBitmapSourceFromArray(byte[] data, Size2D size)
        {
            PixelFormat format = PixelFormats.Rgb24;
            
            WriteableBitmap wbm = new WriteableBitmap(size.Width, size.Height, 96, 96, format, null);
            wbm.WritePixels(new Int32Rect(0, 0, size.Width, size.Height), data, (format.BitsPerPixel / 8) * size.Width, 0);

            return wbm;
        }

    }
}
