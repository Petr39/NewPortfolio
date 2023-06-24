
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;
using System.Linq;
using X.PagedList;


namespace NewPortfolio.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        private IWebHostEnvironment webHostEnvironment;
        public ArticlesController(ApplicationDbContext context,
               UserManager<AppUser> userManager,
               IWebHostEnvironment webHostEnvironment
              )
        {
            _context = context;
            _userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchby, string searchfor, int? page)
        {
            
            var applicationDbContext =_context.Article.Include(a => a.ApplicationUser);
            var applicationDbContex = GetAllArticles(searchby, searchfor).ToPagedList(page ?? 1, 6);
            return View(applicationDbContex);
        }

        private List<Article> GetAllArticles(string searchby, string searchfor)
        {
            if (searchby == "title" && searchfor != null)
            {
                var applicationDbContexta = _context.Article.Include(a => a.ApplicationUser).Include(b=>b.BuildPost).Where(s => s.Title.ToLower().Contains(searchfor.ToLower()));

                return applicationDbContexta.ToList();
            }

            if (searchby == "description" && searchfor != null)
            {
                var applicationDbContexta = _context.Article.Include(a => a.ApplicationUser).Include(b=>b.BuildPost).Where(s => s.Description.ToLower().Contains(searchfor.ToLower()));

                return applicationDbContexta.ToList();
            }

            var applicationDbContext = _context.Article.Include(a => a.ApplicationUser).Include(b=>b.BuildPost);

            return applicationDbContext.ToList();
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Article == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize]        
        //[Area("Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            //ViewData["BuildPostId"] = BuildPosted();
            ViewData["BuildPostId"] = new SelectList(_context.BuildPosts, "Id", "BuildName");
            
            return View();
        }
        private List<SelectListItem> BuildPosted()
        {
            var listPost=new List<SelectListItem>();
            List<BuildPost> buildPosts = _context.BuildPosts.ToList();
            listPost = _context.BuildPosts.Select(x => new SelectListItem(x.BuildName, x.Id.ToString())).ToList();
            return listPost;
        }

        [Authorize] 
        //[Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Title,Description,BuildPostId")] CreatePostVM article)
        {
             var userLog = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
             ViewData["BuildPostId"] = new SelectList(_context.BuildPosts, "Id", "BuildName", article.BuildPostId);

            if (ModelState.IsValid)
            {
                userLog.CountPost += 1;
                var post = new Article();
                                   
                   post.Id = article.Id;
                   post.Title = article.Title;
                   post.Description = article.Description;
                   post.Content = article.Content;
                   post.AppUserId = userLog.Id;
                   post.NickName = userLog.NickName;
                   post.ImageUrl = userLog.Path;
                   post.Credits = userLog.Credit;
                   post.DateOfRegister = userLog.DateOfRegister.ToString("dd.MM.yyyy");
                   post.BuildPostId = article.BuildPostId;                
                   await _context.Article!.AddAsync(post);
                   await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [Authorize]
        //[Area("admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Article == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", article.AppUserId);
            return View(article);
        }

        [Authorize]
      //  [Area("admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Title,Description,AppUserId, NickName")] Article article)
        {
            var userLog = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    article.NickName = userLog.NickName;//Uloží přezdívku podle id-name
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", article.AppUserId);
            return View(article);
        }


        [Authorize]
        //[Area("admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Article == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }


        [Authorize]
        //[Area("admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Article == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Article'  is null.");
            }
            var article = await _context.Article.FindAsync(id);
            if (article != null)
            {
                _context.Article.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return (_context.Article?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "ImagesThumb";

            var folderPath = Path.Combine(webHostEnvironment.WebRootPath, "ImagesThumb");
            uniqueFileName = new Guid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                file.CopyToAsync(fs);
            }

            return uniqueFileName;
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var applicationDbContext = _context.Article.Include(a => a.ApplicationUser);
            return Json(new { data = applicationDbContext });

        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var applicationDbContext = _context.Article.Include(a => a.ApplicationUser);
        //    return Json(new { data = applicationDbContext });

        //}


        #endregion
    }
}
