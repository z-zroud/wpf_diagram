using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfDiagram.Helper
{
    public static class BitmapHelper
    {

        /// <summary>
        /// 截图转换成bitmap
        /// </summary>
        /// <param name="element"></param>
        /// <param name="width">默认控件宽度</param>
        /// <param name="height">默认控件高度</param>
        /// <param name="x">默认0</param>
        /// <param name="y">默认0</param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this FrameworkElement element, int width = 0, int height = 0, int x = 0, int y = 0)
        {
            if (width == 0) width = (int)element.ActualWidth;
            if (height == 0) height = (int)element.ActualHeight;

            var rtb = new RenderTargetBitmap(width, height, x, y, System.Windows.Media.PixelFormats.Default);
            rtb.Render(element);
            var bit = BitmapSourceToBitmap(rtb);

            return bit;
        }


        public static Bitmap ToBitmap(this System.Windows.Media.VisualBrush brush, Rect rect)
        {
            var visual = new System.Windows.Media.DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(brush, null, rect);
            }

            var rtb = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, System.Windows.Media.PixelFormats.Default);
            rtb.Render(visual);

            var bit = BitmapSourceToBitmap(rtb);

            return bit;
        }


        /// <summary>
        /// BitmapSource转Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bitmap BitmapSourceToBitmap(this BitmapSource source)
        {
            return BitmapSourceToBitmap(source, source.PixelWidth, source.PixelHeight);
        }

        /// <summary>
        /// Convert BitmapSource to Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bitmap BitmapSourceToBitmap(this BitmapSource source, int width, int height)
        {
            Bitmap bmp = null;
            try
            {
                PixelFormat format = PixelFormat.Format24bppRgb;
                /*set the translate type according to the in param(source)*/
                switch (source.Format.ToString())
                {
                    case "Rgb24":
                    case "Bgr24": format = PixelFormat.Format24bppRgb; break;
                    case "Bgra32": format = PixelFormat.Format32bppPArgb; break;
                    case "Bgr32": format = PixelFormat.Format32bppRgb; break;
                    case "Pbgra32": format = PixelFormat.Format32bppArgb; break;
                }
                bmp = new Bitmap(width, height, format);
                BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
                    ImageLockMode.WriteOnly,
                    format);
                source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bmp.UnlockBits(data);
            }
            catch
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
            }

            return bmp;
        }

        public static System.Windows.Media.ImageSource ToBitmapSource(this Bitmap bitmap, int width, int height)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.DecodePixelWidth = width;//需要缩略图的解码宽度
            bitmapImage.DecodePixelHeight = height;//缩略图的解码高度
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }       

        public static System.Windows.Media.Brush ToBrush(this string base64String, int width, int height)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                var bitmapImage = base64String.ToBitmapImage(width, height);

                return new System.Windows.Media.ImageBrush(bitmapImage) { Stretch = System.Windows.Media.Stretch.Uniform };
            }
            catch
            {
                return null;
            }
        }

        public static BitmapImage ToBitmapImage(this string base64String, int width = 0, int height = 0)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                var byteArray = Convert.FromBase64String(base64String);
                BitmapImage bitmapImage = null;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.DecodePixelWidth = width;//需要缩略图的解码宽度
                bitmapImage.DecodePixelHeight = height;//缩略图的解码高度
                bitmapImage.StreamSource = new MemoryStream(byteArray);
                bitmapImage.EndInit();

                return bitmapImage;
            }
            catch
            {
                return null;
            }
        }

        public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap ImageOriginal, int width = 0, int height = 0)
        {
            System.Drawing.Bitmap ImageOriginalBase = new System.Drawing.Bitmap(ImageOriginal);
            BitmapImage bitmapImage = new BitmapImage() { DecodePixelWidth = width, DecodePixelHeight = height };
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ImageOriginalBase.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        public static BitmapImage ToBitmapImage(this byte[] byteArray, int width = 0, int height = 0)
        {
            BitmapImage bitmapImage = new BitmapImage() { DecodePixelWidth = width, DecodePixelHeight = height };
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;

        }

        public static Bitmap ToBitmap(this string base64String, int width = 0, int height = 0)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                var byteArray = Convert.FromBase64String(base64String);               

                return byteArray.ToBitmap();
            }
            catch
            {
                return null;
            }
        }

        public static Bitmap ToBitmap(this byte[] byteArray)
        {
            //var stream = new MemoryStream(byteArray);
            //return new Bitmap(stream);
            MemoryStream ms = new MemoryStream(byteArray);
            Bitmap bitmap = (Bitmap)Image.FromStream(ms);
            ms.Close();

            return bitmap;
        }

        public static string ToBase64String(this System.Windows.Media.Brush brush)
        {
            try
            {
                if (brush is System.Windows.Media.VisualBrush visualBrush)
                {
                    var size = ((UIElement)visualBrush.Visual).DesiredSize;
                    var image = visualBrush.ToBitmap(new Rect(size));
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Bmp);
                    stream.Position = 0;
                    var bytearray = stream.ToArray();
                    return Convert.ToBase64String(bytearray);
                }
                else if (brush is System.Windows.Media.ImageBrush imageBrush)
                {
                    var bitmap = (imageBrush.ImageSource as BitmapImage);
                    byte[] bytearray = null;
                    Stream smarket = bitmap.StreamSource;
                    if (smarket != null && smarket.Length > 0)
                    {
                        //设置当前位置
                        smarket.Position = 0;
                        using (BinaryReader br = new BinaryReader(smarket))
                        {
                            bytearray = br.ReadBytes((int)smarket.Length);
                        }
                    }
                    return Convert.ToBase64String(bytearray);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string ToBase64String(this BitmapImage bitmap)
        {
            Stream stream = bitmap.StreamSource;
            byte[] bytearray = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytearray, 0, bytearray.Length);

            return Convert.ToBase64String(bytearray);
        }

        public static string ToBase64String(this Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, bitmap.RawFormat);
            byte[] bytearray = new byte[ms.Length];
            bytearray = ms.ToArray();

            return Convert.ToBase64String(bytearray);
        }


        public static string ToBase64String(this Stream stream)
        {
            byte[] bytearray = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytearray, 0, bytearray.Length);

            return Convert.ToBase64String(bytearray);
        }

        public static MemoryStream ToMemoryStream(this string base64String, int width = 0, int height = 0)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                var byteArray = Convert.FromBase64String(base64String);
                var stream = new MemoryStream(byteArray);
                stream.Seek(0, SeekOrigin.Begin);

                return stream;
            }
            catch
            {
                return null;
            }
        }

        public static FileStream ToFileStream(this string base64String, string filename, int width = 0, int height = 0)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                var byteArray = Convert.FromBase64String(base64String);
                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Write(byteArray, 0, byteArray.Length);
                fs.Seek(0, SeekOrigin.Begin);
                return fs;
            }
            catch
            {
                return null;
            }
        }

        public static Stream CopyStream(this Stream stream)
        {
            byte[] bytearray = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytearray, 0, bytearray.Length);

            var destination = new MemoryStream(bytearray);
            return destination;
        }
    }
}
