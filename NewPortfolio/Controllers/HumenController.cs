using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;
using NewPortfolio.Models.Humans;

namespace NewPortfolio.Controllers
{
    public class HumenController : Controller
    {
        private readonly ApplicationDbContext _context;


       

        public HumenController(ApplicationDbContext context)
        {
            _context = context;
          
        }

        // GET: Humen
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Humans.Include(h => h.Article);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Humen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Humans == null)
            {
                return NotFound();
            }

            var human = await _context.Humans
                .Include(h => h.Article)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (human == null)
            {
                return NotFound();
            }

            return View(human);
        }

        // GET: Humen/Create
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Article, "Id", "Title");
            return View();
        }

        // POST: Humen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ArticleId")] Human human)
        {
            //chybí validace
                _context.Add(human);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
            //ViewData["ArticleId"] = new SelectList(_context.Article, "Id", "Content", human.ArticleId);
            return View(human);
        }

        // GET: Humen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Humans == null)
            {
                return NotFound();
            }

            var human = await _context.Humans.FindAsync(id);
            if (human == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Article, "Id", "Title", human.ArticleId);
            return View(human);
        }

        // POST: Humen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ArticleId")] Human human)
        {
            if (id != human.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(human);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HumanExists(human.Id))
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
            ViewData["ArticleId"] = new SelectList(_context.Article, "Id", "Title", human.ArticleId);
            return View(human);
        }

        // GET: Humen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Humans == null)
            {
                return NotFound();
            }

            var human = await _context.Humans
                .Include(h => h.Article)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (human == null)
            {
                return NotFound();
            }

            return View(human);
        }

        // POST: Humen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Humans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Humans'  is null.");
            }
            var human = await _context.Humans.FindAsync(id);
            if (human != null)
            {
                _context.Humans.Remove(human);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HumanExists(int id)
        {
          return (_context.Humans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
