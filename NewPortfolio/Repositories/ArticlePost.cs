//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using NewPortfolio.Data;
//using NewPortfolio.Models;

//namespace NewPortfolio.Repositories
//{
//    public class ArticlePost : Controller, IArticlePost
//    {

//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<AppUser> _userManager;

//        public ArticlePost(ApplicationDbContext context, UserManager<AppUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }


//        public async Task<IEnumerable<Article>> Index()
//        {
//            var applicationDbContext = _context.Article.Include(a => a.ApplicationUser);
//            return (await applicationDbContext.ToListAsync());
//        }
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.Article == null)
//            {
//                return NotFound();
//            }

//            var article = await _context.Article
//                .Include(a => a.ApplicationUser)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (article == null)
//            {
//                return NotFound();
//            }

//            return View(article);
//        }
//        public async Task<IActionResult> Create([Bind("Id,Content,Title,Description")] CreatePostVM article)
//        {
//            //Najde uzivatele podle id-Name
//            var userLog = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);

//            if (ModelState.IsValid)
//            {
//                var post = new Article();
//                post.Id = article.Id;
//                post.Title = article.Title;
//                post.Description = article.Description;
//                post.Content = article.Content;
//                post.AppUserId = userLog.Id;
//                post.NickName = userLog.NickName;//Uloží přezdívku podle id-name



//                await _context.Article!.AddAsync(post);
//                await _context.SaveChangesAsync();


//                return RedirectToAction(nameof(Index));

//            }

//            return View();
//        }
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.Article == null)
//            {
//                return NotFound();
//            }

//            var article = await _context.Article.FindAsync(id);
//            if (article == null)
//            {
//                return NotFound();
//            }
//            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", article.AppUserId);
//            return View(article);
//        }

//        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Title,Description,AppUserId, NickName")] Article article)
//        {
//            var userLog = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);



//            if (id != article.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    article.NickName = userLog.NickName;//Uloží přezdívku podle id-name

//                    _context.Update(article);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ArticleExists(article.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", article.AppUserId);
//            return View(article);
//        }

//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.Article == null)
//            {
//                return NotFound();
//            }

//            var article = await _context.Article
//                .Include(a => a.ApplicationUser)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (article == null)
//            {
//                return NotFound();
//            }

//            return View(article);
//        }

//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.Article == null)
//            {
//                return Problem("Entity set 'ApplicationDbContext.Article'  is null.");
//            }
//            var article = await _context.Article.FindAsync(id);
//            if (article != null)
//            {
//                _context.Article.Remove(article);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        #region
//        private bool ArticleExists(int id)
//        {
//            return (_context.Article?.Any(e => e.Id == id)).GetValueOrDefault();
//        }

//        #endregion
//    }
//}
