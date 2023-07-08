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
    public class ArticlePostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ArticlePostsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]      
        public async Task<IActionResult> Index(int? id)
        {
            var postList=_context.ArticlePosts;
            return View(await postList.ToListAsync());
        }

        // GET: ArticlePosts/Details/5
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Post,ArticleId")] ArticlePostVM articlePost)
        {

            var user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);


            var a = new ArticlePost()
            {
                ArticleId = articlePost.Id,
                Post = articlePost.Post,
                AppUserId = user.Id,
                UserName = user.NickName,
                DateTime= DateTime.Now,
                
                
            };

            if (ModelState.IsValid)
            {
                _context.Add(a);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(a);
        }
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ArticlePosts == null)
            {
                return NotFound();
            }

            var articlePost = await _context.ArticlePosts.FindAsync(id);
            if (articlePost == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Article, "Id", "Content", articlePost.ArticleId);
            return View(articlePost);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Post,ArticleId")] ArticlePost articlePost)
        {
            if (id != articlePost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articlePost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticlePostExists(articlePost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleId"] = new SelectList(_context.Article, "Id", "Content", articlePost.ArticleId);
            return View(articlePost);
        }

        
        //// GET: ArticlePosts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.ArticlePosts == null)
        //    {
        //        return NotFound();
        //    }

        //    var articlePost = await _context.ArticlePosts
        //        .Include(a => a.Article)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (articlePost == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(articlePost);
        //}

        //// POST: ArticlePosts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.ArticlePosts == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.ArticlePosts'  is null.");
        //    }
        //    var articlePost = await _context.ArticlePosts.FindAsync(id);
        //    if (articlePost != null)
        //    {
        //        _context.ArticlePosts.Remove(articlePost);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ArticlePostExists(int id)
        {
          return (_context.ArticlePosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
