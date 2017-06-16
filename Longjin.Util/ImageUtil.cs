/*
* 图像工具类
* 田野 2015-11-4
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Longjin.Util
{
    /// <summary>
    /// 图像工具类
    /// </summary>
    public sealed class ImageUtil
    {
        /// <summary>
        /// 图片相关公共方法
        /// </summary>
        private ImageUtil() { }

        #region 缩略图代码
        /// <summary> 
        /// 生成jpg缩略图字节
        /// </summary> 
        /// <param name="originalImagePath">原始路径</param> 
        /// <param name="quality">质量0-100</param> 
        /// <param name="width">宽度</param> 
        /// <param name="height">高度</param> 
        /// <param name="mode">模式：HW,W,H,Cut--->HW:指定高宽缩放（可能变形）;W:指定宽，高按比例 ;H:指定高，宽按比例 ;Cut:指定高宽裁减（不变形） </param> 
        /// <returns></returns> 
        public static byte[] MakeJPGBytes(string originalImagePath, long quality, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            MemoryStream s = new MemoryStream();
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                EncoderParameters eps = new EncoderParameters(1);
                EncoderParameter ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                eps.Param[0] = ep;
                bitmap.Save(s, GetCodecInfo("image/jpeg"), eps);
                return s.GetBuffer();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                s.Dispose();
                g.Dispose();
            }
        }

        /**/
        /// <summary> 
        /// 保存JPG时用 
        /// </summary> 
        /// <param name="mimeType"></param> 
        /// <returns>得到指定mimeType的ImageCodecInfo</returns> 
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }
        #endregion

        #region 更改图片大小
        /// <summary>
        /// 更改图片大小
        /// </summary>
        /// <param name="inputimage">原图片文件位置</param>
        /// <param name="outimage">更改后图片文件位置</param>
        /// <param name="width">更改后宽度，像素点 0-表示按照高度等比例</param>
        /// <param name="height">更改后高度，像素点 0-表示按照宽度等比例</param>
        /// <returns>
        /// 10000 - 更改成功
        /// 10001 - 原图片文件不存在
        /// 10002 - 宽度不能为负数
        /// 10003 - 高度不能为负数
        /// 10004 - 宽度和高度不能同时设置为0
        /// 10009 - 其他错误
        /// </returns>
        public static int Resize(string inputimage, string outimage, int width, int height)
        {
            int result = 10009;

            if (System.IO.File.Exists(inputimage) == false) result = 10001;
            else if (width < 0) result = 10002;
            else if (height < 0) result = 10003;
            else if ((width == 0) && (height == 0)) result = 10004;
            else
            {
                try
                {
                    new Aspose.Imaging.License().SetLicense(new MemoryStream(Convert.FromBase64String(AsposeLicenseUtil.Key)));

                    if (width != 0 && height != 0)
                    {
                        // 宽度和高度都改变
                        using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(inputimage))
                        {
                            image.Resize(width, height);
                            image.Save(outimage);
                        }
                    }
                    else if (width == 0)
                    {
                        // 高度改变，宽度等比例变化
                        using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(inputimage))
                        {
                            int origin_width = image.Width;
                            int origin_height = image.Height;

                            int new_width = origin_width * height / origin_height;

                            image.Resize(new_width, height);
                            image.Save(outimage);
                        }
                    }
                    else if (height == 0)
                    {
                        // 高度改变，宽度等比例变化
                        using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(inputimage))
                        {
                            int origin_width = image.Width;
                            int origin_height = image.Height;

                            int new_height = origin_height * width / origin_width;

                            image.Resize(width, new_height);
                            image.Save(outimage);
                        }
                    }

                    result = 10000;
                }
                catch
                {

                }

            }

            return result;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageFormat ConvertImageFormat(ImageFormat format)
        {
            if (format.Guid == ImageFormat.Bmp.Guid || format.Guid == ImageFormat.MemoryBmp.Guid)
            {
                return ImageFormat.Bmp;
            }
            else if (format.Guid == ImageFormat.Emf.Guid)
            {
                return ImageFormat.Emf;
            }
            else if (format.Guid == ImageFormat.Exif.Guid)
            {
                return ImageFormat.Exif;
            }
            else if (format.Guid == ImageFormat.Gif.Guid)
            {
                return ImageFormat.Gif;
            }
            else if (format.Guid == ImageFormat.Icon.Guid)
            {
                return ImageFormat.Icon;
            }
            else if (format.Guid == ImageFormat.Jpeg.Guid)
            {
                return ImageFormat.Jpeg;
            }
            else if (format.Guid == ImageFormat.Png.Guid)
            {
                return ImageFormat.Png;
            }
            else if (format.Guid == ImageFormat.Tiff.Guid)
            {
                return ImageFormat.Tiff;
            }
            else if (format.Guid == ImageFormat.Wmf.Guid)
            {
                return ImageFormat.Wmf;
            }
            else
            {
                throw new Exception("不存在的图片枚举值");
            }
        }


        #region 
        #endregion
    }
}
