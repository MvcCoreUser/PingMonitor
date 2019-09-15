using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PingMonitor.BLL.Interfaces;
using PingMonitor.Web.Models;

namespace PingMonitor.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMonitoringService _monitoringService;

        public HomeController(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }
        public async Task<IActionResult> Index()
        {
            var results = await _monitoringService.GetResults();
            if (results.Any())
            {
                return View(results);
            }
            return RedirectToAction("Error");
        }

        public async Task<IActionResult> Details(int id)
        {
            var results = await _monitoringService.GetResults(id);
            if (results.Any())
            {
                return View(results.First());
            }
            return RedirectToAction("Error");
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
