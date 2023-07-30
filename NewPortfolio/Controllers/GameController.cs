using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewPortfolio.Interfaces;
using NewPortfolio.Models;

namespace NewPortfolio.Controllers
{
   
    public class GameController : Controller
    {

        private readonly IGame _gameRepos;


        public GameController(IGame gameRepos)
        {
            _gameRepos = gameRepos;
        }


        /// <summary>
        /// Vypsání všech her
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowAll()
        {
            return View(_gameRepos.GetAll());
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddGame()
        {
            var game = new Game();
            return View(game);
        }


        /// <summary>
        /// Přidá hru do seznamu
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddGame(Game game)
        {
            if (ModelState.IsValid)
            {
                await _gameRepos.Create(game);
                _gameRepos.Save(game);
                return RedirectToAction("ShowAll");
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditGame(int id)
        {
            var game = _gameRepos.Find(id);
            return View(game);
        }

        /// <summary>
        /// Upraví hru
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditGame(Game game)
        {
            if (ModelState.IsValid)
            {
                await _gameRepos.Update(game);
                _gameRepos.Save(game);
                return RedirectToAction("ShowAll");
            }
            return View(game);
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteGame(int id)
        {
            var game = _gameRepos.Find(id);
            return View(game);
        }

        /// <summary>
        /// Smaže daný typ hry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult DeleteGamePost(int id)
        {
            if (id == null || id==0)
                return RedirectToAction("ShowAll");

            if (ModelState.IsValid)
            {
                _gameRepos.Delete(id);
                TempData["error"] = "Hra smazána";
                return RedirectToAction("ShowAll");
            }

            return RedirectToAction("ShowAll");
        }

        [HttpGet]
        
        public IActionResult GetArticle(int id)
        {
            return View(_gameRepos.GetArticleList(id));
        }
    }
}
