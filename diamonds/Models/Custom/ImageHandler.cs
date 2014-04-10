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

        private int quality = 100;
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
        public void SaveImage(int maxWidth, int maxHeight)
        {
            float ratioX = (float)maxWidth / (float)originalWidth; 
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            var newWidth = (int)(originalWidth * ratio);
            var newHeight = (int)(originalHeight * ratio);

            // Processing image if original file is biger than new dimensions, else save original file
            if (originalWidth > newWidth || originalHeight > newHeight)
                ProcessImage(newWidth, newHeight, quality);
            else
                image.Save(savingPath);
        }

        public void SaveThumbnail()
        {
            var maxWidth = 200;
            var maxHeight = 200;

            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Max(ratioX, ratioY);
            

            x = (int)((originalWidth - maxWidth / ratio) / 2);
            y = (int)((originalHeight - maxHeight / ratio) / 2);

            image = cropImage(image, new Rectangle(x, y, (int)(maxWidth / ratio), (int)(maxHeight / ratio)));

            ProcessImage(maxWidth, maxHeight, quality);
        }
        
            
        private void ProcessImage(int newWidth, int newHeight, int quality)
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

            // Get an ImageCodecInfo object that represents the JPEG codec.
            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);

            // Create an Encoder object for the Quality parameter.
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(savingPath, imageCodecInfo, encoderParameters);
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
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