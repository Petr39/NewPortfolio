using Microsoft.AspNetCore.Mvc;

using NewPortfolio.Models;
using System.Diagnostics;

namespace NewPortfolio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
        
          
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Experience()
        {
            return View();
        }


        public IActionResult RegisterAgree()
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