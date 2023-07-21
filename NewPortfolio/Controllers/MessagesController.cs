using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewPortfolio.Data;

using NewPortfolio.Migrations;
using NewPortfolio.Models;
using NewPortfolio.ModelView;

namespace NewPortfolio.Controllers
{
    public class MessagesController : Controller 
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Vypíše mi seznam zpráv daného uživatele
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string id)
        {
            //Tohle mi poradil chatbot - zkrácení načtení uživatele
            var userLog = await _userManager.GetUserAsync(User);


            var user= await _userManager.FindByIdAsync(id);
            if (user != null && userLog!=null)
            {
                if(user!=userLog)
                    return RedirectToAction("Index","Articles");
                //Vybere mi zprávy podle id uživatele do listu
                var message =await _context.Messages.Where(u=>u.UserId == user.Id).ToListAsync();
                
                return View(message);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(string? id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user != null)
            {
                var message = new Message();
                message.UserId = user.Id;
                return View(message);
            }
            return View(); 
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Message message)
        {
            if(message != null)
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                TempData["success"] = "Zpráva odeslána";
                return RedirectToAction("Index","Articles");
            }
            return View(message);
        }
    }
}
