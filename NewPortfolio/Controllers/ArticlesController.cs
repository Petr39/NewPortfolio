 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;

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

        // GET: Articles
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.Article.Include(a => a.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Articles/Details/5
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

        // GET: Articles/Create
        public IActionResult Create()
        {
            // ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [Authorize]
        //[Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Title,Description")] CreatePostVM article)
        {
            
            var userLog = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);

            if (ModelState.IsValid)
            {
                var post = new Article();
                post.Id = article.Id;
                post.Title = article.Title;
                post.Description = article.Description;
                post.Content = article.Content;
                post.AppUserId = userLog.Id;
                post.NickName = userLog.NickName;
                post.ImageUrl = userLog.Path;
                post.Credits = userLog.Credit;
          

                await _context.Article!.AddAsync(post);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));

            }

            return View();


        }


        [Authorize]
       // [Area("admin")]
        // GET: Articles/Edit/5
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

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        //[Area("admin")]
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

        // GET: Articles/Delete/5
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
                //file.CopyToAsync(fs);
            }

            return uniqueFileName;
      }
    }
}
