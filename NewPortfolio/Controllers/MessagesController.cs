using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;


namespace NewPortfolio.Controllers
{
    //Pro posílání zpráv uživatelům
    [Authorize]
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
        /// <summary>
        /// Vytvoření zprávy uživateli
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Message message)
        {
            var user= await _userManager.GetUserAsync(User);
            if(user != null)
            {
               if(message.MessageBody !=null || message.MessageHead != null)
               {
                   message.DateTime = DateTime.Now;
                   message.UserName = user.NickName;
                   await _context.Messages.AddAsync(message);
                   await _context.SaveChangesAsync();
                   TempData["success"] = "Zpráva odeslána";
                   return RedirectToAction("Index","Articles");
               }
            }
            return View(message);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id == id);
            if(message != null)
            {
                return View(message);
            }

            return NotFound();
        }

        /// <summary>
        /// Smazání zprávy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var message = _context.Messages.FirstOrDefaultAsync(u => u.Id == id);
            if(message != null && user!=null)
            {
                      _context.Remove(message);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
