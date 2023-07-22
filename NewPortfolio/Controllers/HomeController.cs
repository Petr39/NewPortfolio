using Microsoft.AspNetCore.Mvc;

using NewPortfolio.Models;
using System.Diagnostics;

namespace NewPortfolio.Controllers
{

    /// <summary>
    /// Základ pro pohledy
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Základní a zároveň úvodní stránka
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Odkaz - kontakt na tvůrce webové aplikace/fóra
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            return View();
        }


        /// <summary>
        /// Odkaz - zkušenosti 
        /// </summary>
        /// <returns></returns>
        public IActionResult Experience()
        {
            return View();
        }

        /// <summary>
        /// Odkaz - pravidla webové aplikace/fóra
        /// </summary>
        /// <returns></returns>
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