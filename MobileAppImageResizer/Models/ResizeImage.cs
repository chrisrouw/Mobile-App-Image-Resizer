using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MobileAppImageResizer.Models
{
    public class ResizeImage
    {
        [DisplayName("File Name")]
        [Required]
        public string FileName { get; set; }

        [DisplayName("Output File Name")]
        [Required]
        public string OutputFileName { get; set; }

        [Required]
        [DisplayName("Width of Image for App (in pixels)")]
        public int ImageWidth { get; set; }

        [DisplayName("Build Android Images")]
        public bool IncludeAndroid { get; set; }

        [DisplayName("Build iOS Images")]
        public bool IncludeIOS { get; set; }
    }
}