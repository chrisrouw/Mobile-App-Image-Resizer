using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MobileAppImageResizer.Helpers
{
    public class ResizeHelper
    {
        /// <summary>
        /// ldpi        .75x
        /// mdpi        1x
        /// drawable    1x
        /// hdpi        1.5x
        /// xhdpi       2x
        /// xxhdpi      3x
        /// xxxhdpi     4x
        /// </summary>
        public void CreateAppImages(byte[] imageBytes, string fileName, string outputFileName, int width, bool includeAndroid, bool includeIOS)
        {
            Directory.CreateDirectory("output");

            if (includeAndroid)
            {
                // Reference: https://developer.android.com/training/multiscreen/screendensities
                // Width is the baseline width (i.e. mdpi)
                ResizeImage(imageBytes, Convert.ToInt32(width * 0.75), outputFileName, "drawable-ldpi");
                ResizeImage(imageBytes, width, outputFileName, "drawable-mdpi");
                ResizeImage(imageBytes, width, outputFileName, "drawable");
                ResizeImage(imageBytes, Convert.ToInt32(width * 1.5), outputFileName, "drawable-hdpi");
                ResizeImage(imageBytes, width * 2, outputFileName, "drawable-xhdpi");
                ResizeImage(imageBytes, width * 3, outputFileName, "drawable-xxhdpi");
                ResizeImage(imageBytes, width * 4, outputFileName, "drawable-xxxhdpi");
            }

            if (includeIOS)
            {
                ResizeiOSImages(imageBytes, width, fileName, outputFileName, "iOS");
            }
        }

        private void ResizeImage(byte[] imageBytes, int width, string outputFileName, string folder)
        {
            try
            {
                Directory.CreateDirectory("output/" + folder);

                using (Image image = Image.Load(imageBytes))
                {
                    image.Mutate(x => x
                         .Resize(new Size(width, 0)));

                    var fname = Path.Combine(("output/" + folder), outputFileName);
                    image.Save(fname); // Automatic encoder selected based on extension.
                }
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }
        }

        private void ResizeiOSImages(byte[] imageBytes, int width, string fileName, string outputFileName, string folder)
        {
            var extension = Path.GetExtension(fileName);
            for (int i = 1; i <= 3; i++)
            {
                var versionExt = i == 1 ? ".png" : "@" + i + "x.png";
                var fName = outputFileName.Replace(extension, ".png").Replace(".png", versionExt);
                ResizeImage(imageBytes, width * i, fName, folder);
            }
        }
    }
}