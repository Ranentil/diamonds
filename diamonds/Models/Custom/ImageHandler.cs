using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace Diamonds.Models
{
    /// <summary>
    /// Class contaning method to resize an image and save in JPEG format
    /// </summary>
    public class ImageHandler
    {
        private Image image;
        private string originalPath;
        private string savingPath;

        private int originalWidth;
        private int originalHeight;

        private int quality = 80;
        private int x = 0;
        private int y = 0;

        public ImageHandler(string originalPath, string savingPath)
        {
            this.originalPath = originalPath;
            this.savingPath = savingPath;

            this.image = Image.FromFile(originalPath);

            this.originalWidth = image.Width;
            this.originalHeight = image.Height;
        }

        /// <summary>
        /// Method to resize, convert and save the image.
        /// </summary>
        /// <param name="maxWidth">resize width.</param>
        /// <param name="maxHeight">resize height.</param>
        /// <param name="watermark">draw watermark</param>
        public void SaveImage(int maxWidth, int maxHeight, bool watermark)
        {
            float ratioX = (float)maxWidth / (float)originalWidth; 
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            var newWidth = (int)(originalWidth * ratio);
            var newHeight = (int)(originalHeight * ratio);

            // Processing image if original file is biger than new dimensions, else save original file
            if (originalWidth > newWidth || originalHeight > newHeight)
                resizeImage(newWidth, newHeight);

            drawWatermark(16, 1, 1);

            saveImage(quality);
        }

        /// <summary>
        /// Method to save tumbnail of size 200x200px
        /// </summary>
        public void SaveThumbnail()
        {
            var maxWidth = 200;
            var maxHeight = 200;

            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Max(ratioX, ratioY);
            

            x = (int)((originalWidth - maxWidth / ratio) / 2);
            y = (int)((originalHeight - maxHeight / ratio) / 2);

            cropImage(new Rectangle(x, y, (int)(maxWidth / ratio), (int)(maxHeight / ratio)));

            resizeImage(maxWidth, maxHeight);

            saveImage(quality);
        }

        /// <summary>
        /// Method to save image
        /// </summary>
        /// <param name="quality">image quality max = 100</param>
        private void saveImage(int quality)
        {
            // Get an ImageCodecInfo object that represents the JPEG codec.
            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);

            // Create an Encoder object for the Quality parameter.
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            image.Save(savingPath, imageCodecInfo, encoderParameters);
        }

        /// <summary>
        /// Method to resize Image
        /// </summary>
        /// <param name="newWidth">new width of image</param>
        /// <param name="newHeight">new height of image</param>
        private void resizeImage(int newWidth, int newHeight)
        {
            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            image = (Image)(newImage);
        }

        /// <summary>
        /// Method to crop Image
        /// </summary>
        /// <param name="cropArea"></param>
        private void cropImage(Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(image);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            image = (Image)(bmpCrop);
        }

        /// <summary>
        /// Method to draw watermark softball.pl
        /// </summary>
        /// <param name="fontSize"></param>
        /// <param name="position"></param>
        private void drawWatermark(float fontSize, int x, int y)
        {
            Font crFont = new Font("calibri", fontSize, FontStyle.Bold);

            int yPixlesFromBottom = (int)(image.Height * .05);
            float yPosFromBottom = ((image.Height - yPixlesFromBottom) - (fontSize / 2));
            float xCenterOfImg = (image.Width / 2);

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            Bitmap wmImage = new Bitmap(image);
            using (Graphics graphics = Graphics.FromImage(wmImage))
            {
                SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(120, 0, 0, 0));
                graphics.DrawString("softball.pl", crFont, semiTransBrush2, new PointF(xCenterOfImg + 1, yPosFromBottom + 1), StrFormat);

                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(120, 255, 255, 255));
                graphics.DrawString("softball.pl", crFont, semiTransBrush, new PointF(xCenterOfImg, yPosFromBottom), StrFormat);
            }
            image = (Image)(wmImage);
        }

        /// <summary>
        /// Method to get encoder infor for given image format.
        /// </summary>
        /// <param name="format">Image format</param>
        /// <returns>image codec info.</returns>
        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }
    }
}