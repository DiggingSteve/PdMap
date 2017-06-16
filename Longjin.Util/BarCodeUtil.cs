/*
* 条形码工具类
* 田野 2015-11-4
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;

namespace Longjin.Util
{
    /// <summary>
    /// 条形码工具类
    /// </summary>
    public sealed class BarCodeUtil
    {
        private BarCodeUtil() { }

        /// <summary>
        /// 生成QR二维码
        /// </summary>
        /// <param name="filepath">二维码图片文件地址</param>
        /// <param name="content">二维码内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>
        /// 10000 - 操作成功
        /// 10001 - 操作失败，二维码图片文件地址为空
        /// 10002 - 操作失败，二维码内容为空
        /// 10003 - 操作失败，宽度和高度设置不正确
        /// 10009 - 操作失败，其他错误
        /// </returns>
        public static int CreateQRbarcode(string filepath, string content, float width, float height)
        {
            int result = 0;

            if (string.IsNullOrEmpty(filepath)) result = 10001;
            else if (string.IsNullOrEmpty(content)) result = 10002;
            else if (width <= 0) result = 10003;
            else if (height <= 0) result = 10003;
            else
            {
                try
                {
                    new Aspose.BarCode.License().SetLicense(new MemoryStream(Convert.FromBase64String(AsposeLicenseUtil.Key)));

                    Aspose.BarCode.BarCodeBuilder builder = new Aspose.BarCode.BarCodeBuilder();
                    builder.SymbologyType = Aspose.BarCode.Symbology.QR;
                    builder.CodeLocation = Aspose.BarCode.CodeLocation.None;
                    builder.CodeText = content;
                    builder.xDimension = width;
                    builder.yDimension = height;
                    builder.Save(filepath);

                    result = 10000;
                }
                catch
                {
                    result = 10009;
                }

            }


            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Bitmap CreateQR(string content, float width, float height)
        {
            new Aspose.BarCode.License().SetLicense(new MemoryStream(Convert.FromBase64String(AsposeLicenseUtil.Key)));
            Aspose.BarCode.BarCodeBuilder builder = new Aspose.BarCode.BarCodeBuilder();
            builder.SymbologyType = Aspose.BarCode.Symbology.QR;
            builder.CodeLocation = Aspose.BarCode.CodeLocation.None;
            builder.QRErrorLevel = Aspose.BarCode.QRErrorLevel.LevelQ;
            builder.CodeText = content;
            builder.xDimension = width;
            builder.yDimension = height;
            return builder.GetOnlyBarCodeImage();
        }
        /// <summary>  
        /// 调用此函数后使此两种图片合并，类似相册，有个  
        /// 背景图，中间贴自己的目标图片  
        /// </summary>  
        /// <param name="bgImg">粘贴的源图片</param>  
        /// <param name="avatar">粘贴的目标图片</param>
        /// <param name="newHeight"></param>
        /// <param name="newWidth"></param>
        public static Image CombinImage(Image bgImg, string avatar, int newWidth = 50, int newHeight = 50)
        {
            Image img = Image.FromFile(avatar);
            img = AddWhiteBorder(img, 30);
            if (img.Height != newWidth || img.Height != 50)
            {
                img = KiResizeImage(img, newWidth, newHeight);
            }
            using (Graphics g = Graphics.FromImage(bgImg))
            {
                g.DrawImage(bgImg, 0, 0, bgImg.Width, bgImg.Height);
                g.DrawImage(img, bgImg.Width / 2 - img.Width / 2, bgImg.Width / 2 - img.Width / 2, img.Width, img.Height);
                return bgImg;
            }
        }

        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="img">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <returns>处理以后的图片</returns>  
        public static Image KiResizeImage(Image img, int newW, int newH)
        {
            Image b = new Bitmap(newW, newH);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            g.Dispose();
            return b;
        }

        /// <summary>
        /// 在图片上方加入白边
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="borderWidth">白边的高度，单位是像素</param>
        /// <returns>Bitmap</returns>
        public static Bitmap AddWhiteBorder(Image img, int borderWidth)
        {
            int bordwidth = borderWidth;
            int bordheight = borderWidth;

            int newheight = img.Height + bordheight;
            int newwidth = img.Width + bordwidth;

            Color bordcolor = Color.White;
            Bitmap bmp = new Bitmap(newwidth, newheight);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            int Style = 0;     //New: 绘制边框的类型, 手动修改0,1,2 可改变边框类型  
            if (Style == 0)   //New: 整个边框.  
            {
                //Changed: 修改rec区域, 将原图缩放. 适合边框内  
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, bordwidth / 2, newwidth - bordwidth / 2, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawRectangle(new Pen(bordcolor, bordheight), 0, 0, newwidth, newheight);
            }
            else if (Style == 1)   //New: 上下边框.  
            {
                System.Drawing.Rectangle rec = new Rectangle(0, bordwidth / 2, newwidth, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, newwidth, 0);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, newheight, newwidth, newheight);
            }
            else if (Style == 2)   //New: 左右边框.  
            {
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, 0, newwidth - bordwidth / 2, newheight);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, 0, newheight);
                g.DrawLine(new Pen(bordcolor, bordheight), newwidth, 0, newwidth, newheight);
            }
            return bmp;
        }
    }
}
