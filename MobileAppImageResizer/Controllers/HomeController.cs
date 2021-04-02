using System.Diagnostics;
using System.Threading.Tasks;
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

        public IActionResult CreateImages()
        {
            var resizeHelper = new ResizeHelper();
            resizeHelper.CreateAppImages("facebook.png", "facebook.png", 36, true, true);
            //resizeHelper.CreateAppImages("logo.png", "logo.png", 200, true, true);
            //resizeHelper.CreateAppImages("close.png", "close.png", 58, true, true);
            //resizeHelper.CreateAppImages("wavy_bottom.png", "wavy_bottom.png", 540, false, true);
            //resizeHelper.CreateAppImages("about_menu.png", "about_menu.png", 111, true, true);
            //resizeHelper.CreateAppImages("discount_menu.png", "discount_menu.png", 111, true, true);
            //resizeHelper.CreateAppImages("home_menu.png", "home_menu.png", 111, true, true);
            //resizeHelper.CreateAppImages("membercard_menu.png", "membercard_menu.png", 111, true, true);

            return RedirectToAction(nameof(Index));
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
            //var resourceList = await _resourceListService.Get();
            //ViewData["ResourceListId"] = new SelectList(resourceList, "Id", "Name");
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FileName,OutputFileName,ImageWidth,IncludeAndroid,IncludeIOS")] ResizeImage resizeImage)
        {
            if (ModelState.IsValid)
            {
                var resizeHelper = new ResizeHelper();
                resizeHelper.CreateAppImages(resizeImage.FileName, resizeImage.OutputFileName, resizeImage.ImageWidth, resizeImage.IncludeAndroid, resizeImage.IncludeIOS);

                return RedirectToAction(nameof(Index));
            }

            return View(resizeImage);
        }
    }
}