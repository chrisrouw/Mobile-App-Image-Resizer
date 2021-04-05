using System.Diagnostics;
using System.IO;
using Ionic.Zip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAppImageResizer.Helpers;
using MobileAppImageResizer.Models;

namespace MobileAppImageResizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return View(new ResizeImage { IncludeAndroid = true, IncludeIOS = true });
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FileName,OutputFileName,ImageWidth,IncludeAndroid,IncludeIOS")] ResizeImage resizeImage)
        {
            if (!resizeImage.IncludeIOS && !resizeImage.IncludeAndroid)
            {
                ModelState.AddModelError(nameof(ResizeImage.IncludeAndroid), "Android or iOS must be selected");
            }

            if (ModelState.IsValid)
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes("Images/" + resizeImage.FileName);

                var resizeHelper = new ResizeHelper();
                var userDirectory = resizeHelper.CreateAppImages(imageBytes, resizeImage.FileName, resizeImage.OutputFileName, resizeImage.ImageWidth, resizeImage.IncludeAndroid, resizeImage.IncludeIOS);
                if (!string.IsNullOrWhiteSpace(userDirectory))
                {
                    var zippedFileName = $"{userDirectory}.zip";
                    ZipAndReturnFiles(userDirectory, zippedFileName);

                    // Download the file
                }

                return RedirectToAction(nameof(Index));
            }

            return View(resizeImage);
        }

        private void ZipAndReturnFiles(string userDirectory, string fileToCreate)
        {
            using (ZipFile zipFile = new ZipFile())
            {
                foreach (string folder in Directory.GetDirectories(userDirectory))
                {
                    zipFile.AddDirectoryByName(folder);

                    foreach (string filename in Directory.GetFiles(folder))
                    {
                        zipFile.AddFile(filename, folder);
                    }
                }
                zipFile.Save(fileToCreate);
            }
        }
    }
}