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

        public IActionResult ShowAll()
        {
            return View(_gameRepos.GetAll());
        }


        [HttpGet]
        public IActionResult AddGame()
        {
            var game = new Game();
            return View(game);
        }

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


        [HttpGet]
        public IActionResult EditGame(int id)
        {
            var game = _gameRepos.Find(id);
            return View(game);
        }

        [HttpPost]
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
        public IActionResult DeleteGame(int id)
        {
            var game = _gameRepos.Find(id);
            return View(game);
        }


        [HttpPost]
        public IActionResult DeleteGamePost(int id)
        {
            if (ModelState.IsValid)
            {
                _gameRepos.Delete(id);
                TempData["error"] = "Hra smazána";
                return RedirectToAction("ShowAll");
            }

            return RedirectToAction("ShowAll");
        }
    }
}
