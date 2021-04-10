using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Ionic.Zip;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Create([Bind("FileName,OutputFileName,ImageWidth,IncludeAndroid,IncludeIOS")] ResizeImage resizeImage, List<IFormFile> files)
        {
            if (!resizeImage.IncludeIOS && !resizeImage.IncludeAndroid)
            {
                ModelState.AddModelError(nameof(ResizeImage.IncludeAndroid), "Android or iOS must be selected");
            }

            if (files.Count == 0)
            {
                ModelState.AddModelError(nameof(ResizeImage.OutputFileName), "You must upload at least one image");
            }
            else
            {
                var fileToCheck = files[0];
                var extension = Path.GetExtension(fileToCheck.FileName).ToLower();
                if (!extension.Contains("png") && !extension.Contains("jpg"))
                {
                    ModelState.AddModelError(nameof(ResizeImage.OutputFileName), "File must be JPG or PNG.");
                }
            }

            if (ModelState.IsValid)
            {
                var resizeHelper = new ResizeHelper();

                // Should only be one file
                var formFile = files[0];
                if (formFile.Length > 0)
                {
                    var fileName = formFile.FileName;

                    var outputFileName = resizeImage.OutputFileName;
                    if (Path.GetExtension(outputFileName).Length == 0)
                    {
                        outputFileName += Path.GetExtension(fileName);
                    }

                    using (var ms = new MemoryStream())
                    {
                        formFile.CopyTo(ms);
                        var imageBytes = ms.ToArray();

                        var userDirectory = resizeHelper.CreateAppImages(imageBytes, fileName, outputFileName, resizeImage.ImageWidth, resizeImage.IncludeAndroid, resizeImage.IncludeIOS);
                        if (!string.IsNullOrWhiteSpace(userDirectory))
                        {
                            var zippedFileName = $"{userDirectory}.zip";
                            ZipAndReturnFiles(userDirectory, zippedFileName);

                            // Download the file if it exists
                            if (System.IO.File.Exists(zippedFileName))
                            {
                                var memory = new MemoryStream();
                                using (var stream = new FileStream(zippedFileName, FileMode.Open, FileAccess.Read))
                                {
                                    stream.CopyTo(memory);
                                }

                                memory.Position = 0;
                                return new FileStreamResult(memory, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"))
                                {
                                    FileDownloadName = "images.zip"
                                };
                            }
                        }
                    }
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