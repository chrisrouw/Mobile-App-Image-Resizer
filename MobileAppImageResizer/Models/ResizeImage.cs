using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MobileAppImageResizer.Models
{
    public class ResizeImage
    {
        [DisplayName("Output File Name - include the extension (e.g. image.png)")]
        [Required]
        public string OutputFileName { get; set; }

        [Required]
        [DisplayName("Image Width for use in your mobile app (in pixels)")]
        public int ImageWidth { get; set; }

        [DisplayName("Create Android Images")]
        public bool IncludeAndroid { get; set; }

        [DisplayName("Create iOS Images")]
        public bool IncludeIOS { get; set; }
    }
}