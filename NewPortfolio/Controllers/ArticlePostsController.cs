using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;

namespace NewPortfolio.Controllers
{   
    /// <summary>
    /// Třída určneá k posílání zpráv uživatelům v příspěvcích
    /// </summary>
    public class ArticlePostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ArticlePostsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        /// <summary>
        /// Načtení všech zpráv
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]      
        public async Task<IActionResult> Index(int? id)
        {
            var postList=_context.ArticlePosts;
            return View(await postList.ToListAsync());
        }

        /// <summary>
        /// Detaily zprávy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ArticlePosts == null)
            {
                return NotFound();
            }

            var articlePost = await _context.ArticlePosts
                .Include(a => a.Article)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articlePost == null)
            {
                return NotFound();
            }

            return View(articlePost);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var art = new ArticlePostVM();
            return View(art);
        }


        /// <summary>
        /// Vytvoření zprávy s názvem uživatele a jeho id
        /// </summary>
        /// <param name="articlePost"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Post,ArticleId")] ArticlePostVM articlePost)
        {
            //Načtení přihlášeného uživatele
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);

            if (user != null)
            {
                var a = new ArticlePost()
                {
                    ArticleId = articlePost.Id,
                    Post = articlePost.Post,
                    AppUserId = user.Id,
                    UserName = user.NickName,
                    DateTime= DateTime.Now,
                };

                //Ověření, jestli je validace v pořádku
               if (ModelState.IsValid)
               {
                     _context.Add(a);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Přidán komentář článku";
                    return RedirectToAction("Index");
               }
            

            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var article= new ArticlePost();
            if (id != null)
            {
                 article=_context.ArticlePosts.FirstOrDefault(x => x.Id == id);
            }
            return View(article);
        }

        public IActionResult Edit(int? id, ArticlePostVM articlePost)
        {

            var user = _userManager.Users.FirstOrDefault(u=>u.UserName == User.Identity!.Name);

            if(ModelState.IsValid && user != null)
            {
               var articlePostAdd = new ArticlePost()
               {
                   ArticleId = articlePost.ArticleId,
                   Post = articlePost.Post,
                   Id=articlePost.Id,
                   DateTime = DateTime.Now,
                   UserName=user.NickName,
                   AppUserId= user.Id,
               };

                _context.Update(articlePostAdd);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articlePost);
        }


       /// <summary>
       /// Smazání komentáře článku
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ArticlePosts == null)
            {
                return NotFound();
            }

            var articlePost = await _context.ArticlePosts
                .Include(a => a.Article)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articlePost == null)
            {
                return NotFound();
            }

            return View(articlePost);
        }

        /// <summary>
        /// Potvrzení smazání komentáře k článku
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ArticlePosts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ArticlePosts'  is null.");
            }
            var articlePost = await _context.ArticlePosts.FindAsync(id);
            if (articlePost != null)
            {
                _context.ArticlePosts.Remove(articlePost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        /// <summary>
        /// Ověření, že existuje příspěvek 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ArticlePostExists(int id)
        {
          return (_context.ArticlePosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
