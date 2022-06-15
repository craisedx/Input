using System.Diagnostics;
using Input.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Input.Models;

namespace Input.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFanFictionService fanFictionService;
        private readonly IAdminService adminService;
        public HomeController(ILogger<HomeController> logger, IFanFictionService fanFictionService,IAdminService adminService)
        {
            _logger = logger;
            this.fanFictionService = fanFictionService;
            this.adminService = adminService;
        }

        public IActionResult Index()
        {
            var bestLikeFanFiction = fanFictionService.GetBestPostByUniqueFandom(6);
            
            ViewBag.BestFandomsByLikes = bestLikeFanFiction;

            var popularFandoms = fanFictionService.GetBestFandoms(6);
            
            ViewBag.BestFandoms = popularFandoms;

            var lastAddedAndApproved = fanFictionService.GetLastAddedAndApproved(6);
            
            return View(lastAddedAndApproved);
        }

        public IActionResult AccessDenied(string ReturnUrl = null)
        {
            return View();
        }

        public IActionResult Rules()
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
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}