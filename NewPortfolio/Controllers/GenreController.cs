using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewPortfolio.Data;
using NewPortfolio.Interfaces;
using NewPortfolio.Models;

namespace NewPortfolio.Controllers
{
    public class GenreController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly IGenre _genre;

        public GenreController(IGenre genre, ApplicationDbContext context)
        {
            _genre = genre;
            _context = context;
        }

        /// <summary>
        /// Seznam žánrů
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewAllGenres()
        {
            return View(_genre.GetAll());
        }



        [HttpGet]
        public IActionResult CreateGenre()
        {

            var articles = _context.Article.ToList();
            ViewData["article"] = new SelectList(_context.Article, "Id", "Description");
            ViewData["game"] = new SelectList(_context.Games, "Id", "GameName");

            var obj=new Genre();
            return View(obj);
        }

        [HttpPost]
        public IActionResult CreateGenre(Genre genre)
        {
            ViewData["article"] = new SelectList(_context.Games, "Id", "GameName", genre.Games);
            if (ModelState.IsValid)
            {
                _genre.Create(genre);
                return RedirectToAction("ViewAllGenres");
            }  
            return View();
        }
    }
}
