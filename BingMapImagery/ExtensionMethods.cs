
namespace BingMapImagery
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    internal static class ExtensionMethods
    {
        public static Image ReSize(this Image srcImage, double resizeFactor)
        {
            if (srcImage == null)
            {
                throw new ArgumentNullException("srcImage");
            }

            if (resizeFactor <= 0)
            {
                throw new ArgumentException("resizeFactor must be greater than zero");
            }

            if (resizeFactor == 1)
            {
                return srcImage;
            }

            // TODO: investigate other approaches of increasing image size.
            var width = srcImage.Width * resizeFactor;
            var height = srcImage.Height * resizeFactor;
            var newImage = new Bitmap((int)width, (int)height);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighSpeed;
                gr.InterpolationMode = InterpolationMode.High;
                gr.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                gr.DrawImage(srcImage, new Rectangle(0, 0, (int)width, (int)height));
                gr.Flush();
            }

            return newImage;
        }

        public static byte[] ToByteArray(this Image value, ImageFormat format)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            // TODO: investigate quality/performance in this conversion
            ////ImageConverter converter = new ImageConverter();
            ////byte[] imgArray = (byte[])converter.ConvertTo(imageIn, typeof(byte[]));

            var stream = new MemoryStream();
            value.Save(stream, format);
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}