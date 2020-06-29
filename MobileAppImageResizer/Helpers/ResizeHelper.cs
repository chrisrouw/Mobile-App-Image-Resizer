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
        public void CreateAppImages(string fileName, string outputFileName, int width, bool includeAndroid, bool includeIOS)
        {
            Directory.CreateDirectory("output");

            if (includeAndroid)
            {
                // Reference: https://developer.android.com/training/multiscreen/screendensities
                // Width is the baseline width (i.e. mdpi)
                ResizeImage(Convert.ToInt32(width * 0.75), fileName, outputFileName, "drawable-ldpi");
                ResizeImage(width, fileName, outputFileName, "drawable-mdpi");
                ResizeImage(width, fileName, outputFileName, "drawable");
                ResizeImage(Convert.ToInt32(width * 1.5), fileName, outputFileName, "drawable-hdpi");
                ResizeImage(width * 2, fileName, outputFileName, "drawable-xhdpi");
                ResizeImage(width * 3, fileName, outputFileName, "drawable-xxhdpi");
                ResizeImage(width * 4, fileName, outputFileName, "drawable-xxxhdpi");
            }

            if (includeIOS)
            {
                ResizeiOSImages(width, fileName, outputFileName, "iOS");
            }
        }

        private void ResizeImage(int width, string fileName, string outputFileName, string folder)
        {
            try
            {
                Directory.CreateDirectory("output/" + folder);

                byte[] photoBytes = File.ReadAllBytes("Images/" + fileName);

                using (Image image = Image.Load(photoBytes))
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

        private void ResizeiOSImages(int width, string fileName, string outputFileName, string folder)
        {
            var extension = Path.GetExtension(fileName);
            for (int i = 1; i <= 3; i++)
            {
                var versionExt = i == 1 ? ".png" : "@" + i + "x.png";
                var fName = outputFileName.Replace(extension, ".png").Replace(".png", versionExt);
                ResizeImage(width * i, fileName, fName, folder);
            }
        }
    }
}