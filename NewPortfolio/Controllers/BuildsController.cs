using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;

namespace NewPortfolio.Controllers
{
    public class BuildsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Builds.Where(c=>c.BuildPostId == id).ToListAsync();
            return View(await applicationDbContext);
        }


        [AllowAnonymous]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Builds == null)
            {
                return NotFound();
            }

            var build = await _context.Builds
                .Include(b => b.BuildPost)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (build == null)
            {
                return NotFound();
            }

            return View(build);
        }

       [Authorize]
        public IActionResult Create()
        {
            ViewData["BuildPostId"] = new SelectList(_context.BuildPosts, "Id", "Id");
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BuildPostId")] Build build)
        {
            if (ModelState.IsValid)
            {
                _context.Add(build);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildPostId"] = new SelectList(_context.BuildPosts, "Id", "Id", build.BuildPostId);
            return View(build);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Builds == null)
            {
                return NotFound();
            }

            var build = await _context.Builds.FindAsync(id);
            if (build == null)
            {
                return NotFound();
            }
            ViewData["BuildPostId"] = new SelectList(_context.BuildPosts, "Id", "Id", build.BuildPostId);
            return View(build);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BuildPostId")] Build build)
        {
            if (id != build.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(build);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildExists(build.Id))
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
            ViewData["BuildPostId"] = new SelectList(_context.BuildPosts, "Id", "Id", build.BuildPostId);
            return View(build);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Builds == null)
            {
                return NotFound();
            }

            var build = await _context.Builds
                .Include(b => b.BuildPost)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (build == null)
            {
                return NotFound();
            }

            return View(build);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Builds == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Builds'  is null.");
            }
            var build = await _context.Builds.FindAsync(id);
            if (build != null)
            {
                _context.Builds.Remove(build);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildExists(int id)
        {
          return (_context.Builds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
