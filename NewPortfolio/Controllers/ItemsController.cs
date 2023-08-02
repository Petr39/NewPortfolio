using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;
using X.PagedList;

namespace NewPortfolio.Controllers
{
    /// <summary>
    /// Třída určená k tvoření věcí/itemů ze hry, popisy itemů/věcí
    /// </summary>
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
     
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        /// <summary>
        /// Výpis věech věcí
        /// </summary>
        /// <param name="searchby"></param>
        /// <param name="searchfor"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AllowAnonymous]
        
        public async Task<IActionResult> Index(string searchby, string searchfor, int? page)
        {

           
            var applicationDbContex = GetAllItems(searchby, searchfor).ToPagedList(page ?? 1, 6);
            return View(applicationDbContex);           
        }
        /// <summary>
        /// Vyhledání věcí podle klíčového slova/znaku
        /// </summary>
        /// <param name="searchby"></param>
        /// <param name="searchfor"></param>
        /// <returns></returns>
        private List<Item> GetAllItems(string searchby, string searchfor)
        {
            if (searchby == "nameitem" && searchfor != null)
            {
                var applicationDbContexta = _context.Items.Where(s => s.NameItem.ToLower().Contains(searchfor.ToLower()));

                return  applicationDbContexta.ToList();
            }

            if (searchby == "description" && searchfor != null)
            {
                var applicationDbContexta = _context.Items.Where(s => s.DescriptionItem.ToLower().Contains(searchfor.ToLower()));

                return applicationDbContexta.ToList();
            }
            var app = _context.Items.ToList();
            return app;    
            
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [Authorize]

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameItem,PathItem,DescriptionItem")] Item item, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string itemPath = Path.Combine(wwwRootPath, @"images\Items");

                    using (var fileStream = new FileStream(Path.Combine(itemPath, fileName), FileMode.Create))
                    {
                        file.CopyToAsync(fileStream);
                    }

                    item.PathItem = @"\images\Items\" + fileName;
                    ViewData["img"] = item.PathItem;

                     await _context.Items.AddAsync(item);
                    await _context.SaveChangesAsync();
                    TempData["success"] = $"Předmět {item.NameItem} přidán";
                   return RedirectToAction(nameof(Index));
                }
            
            }
            return View(item);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameItem,PathItem")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new Exception("Problém s editací - Item");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [Authorize(Roles = "admin")]
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
               
                return NotFound();
            }

            return View(item);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                TempData["error"] = $"Předmět smazán";
                return RedirectToAction(nameof(Index));
            }
            
            return View(item);
        }

        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
