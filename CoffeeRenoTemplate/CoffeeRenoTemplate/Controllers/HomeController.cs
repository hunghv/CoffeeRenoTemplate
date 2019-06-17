using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using CoffeeRenoTemplate.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeRenoTemplate.Controllers
{

    public class HomeController : Controller
    {
        [Route("")]
        [Authorize]
        public IActionResult Index()
        {
           var a= User.Identity.Name;
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
    }
}
