using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCoreAppLifetimeEvents.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;

namespace DotNetCoreAppLifetimeEvents.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, IHostApplicationLifetime lifetime, IMemoryCache cache)
        {
            _logger = logger;
            _lifetime = lifetime;
            _cache = cache;
        }

        public IActionResult Index()
        {
            if (_cache.Get("timestamp") == null)
            {
                _cache.Set<string>("timestamp", DateTime.Now.ToString());
            }
            ViewBag.TimeStamp = _cache.Get<string>("timestamp");
            return View();
        }

        [HttpPost]
        public IActionResult Stop()
        {
            _lifetime.StopApplication();
            return new ObjectResult("Application stopped successfully");
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
    }
}
