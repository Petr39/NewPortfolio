using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;

namespace NewPortfolio.Controllers
{
    public class BuildPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuildPosts
        public async Task<IActionResult> Index()
        {
              return _context.BuildPosts != null ? 
                          View(await _context.BuildPosts.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.BuildPosts'  is null.");
        }

        // GET: BuildPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {


           // var log = _context.Builds.First(b => b.Id == id);
            if (id == null || _context.Builds == null)
            {
                return NotFound();
            }

            var buildPost = await _context.Builds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildPost == null)
            {
                return NotFound();
            }

            return View(buildPost);
        }

        // GET: BuildPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BuildPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BuildName")] BuildPost buildPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buildPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buildPost);
        }

        // GET: BuildPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BuildPosts == null)
            {
                return NotFound();
            }

            var buildPost = await _context.BuildPosts.FindAsync(id);
            if (buildPost == null)
            {
                return NotFound();
            }
            return View(buildPost);
        }

        // POST: BuildPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BuildName")] BuildPost buildPost)
        {
            if (id != buildPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buildPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildPostExists(buildPost.Id))
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
            return View(buildPost);
        }

        // GET: BuildPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BuildPosts == null)
            {
                return NotFound();
            }

            var buildPost = await _context.BuildPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildPost == null)
            {
                return NotFound();
            }

            return View(buildPost);
        }

        // POST: BuildPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BuildPosts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BuildPosts'  is null.");
            }
            var buildPost = await _context.BuildPosts.FindAsync(id);
            if (buildPost != null)
            {
                _context.BuildPosts.Remove(buildPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildPostExists(int id)
        {
          return (_context.BuildPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
