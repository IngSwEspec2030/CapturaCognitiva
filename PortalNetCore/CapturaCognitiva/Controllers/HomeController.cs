
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CapturaCognitiva.Models;
using Microsoft.AspNetCore.Identity;
using CapturaCognitiva.Data;

namespace CapturaCognitiva.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {

                return View();
            }
            else
            {                
                return RedirectToAction("Login", "Accounts");
            }
        }
        public IActionResult ForgotPassword()
        {
            if (!_signInManager.IsSignedIn(User))
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
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
