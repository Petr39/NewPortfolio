using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;
using NuGet.Protocol.Plugins;
using System.IO.Compression;

namespace NewPortfolio.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        //private readonly IEmailService emailService;
       

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            //this.emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl =null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

           
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return string.IsNullOrWhiteSpace(returnUrl) ?
                    RedirectToAction("Administration") :
                    RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje");
            return View(model);
        }
      


        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
       

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {



            if(model.Check)
            { 
                ViewData["ReturnUrl"] = returnUrl;

            if (await userManager.FindByEmailAsync(model.Email) is null)
            {
                 var user = new AppUser { UserName = model.Email, Email = model.Email, NickName = model.NickNameUser };
                     //---------Tady bude při registraci overovaci email------------// 
                     // var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                     // var confirmLink = Url.Action(nameof(Confirm),"Authentication",new {token,email = user.Email!},Request.Scheme);
                     // var message = new Message(new string[] { user.Email! }, "Confirmation email link", cofirmLink);
                 var result = await userManager.CreateAsync(user, model.Password);

                    //if (user.TwoFactorEnabled)
                    //{

                    //}


                 if (result.Succeeded)
                 {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return string.IsNullOrWhiteSpace(returnUrl) ?
                    RedirectToAction("Index", "Home") :
                    RedirectToLocal(returnUrl);
                 }

                    AddErrors(result);
             }
                AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Email {model.Email} je již zaregistrován" }));
            }
            return View(model);
        }


        [Authorize]
        public IActionResult Administration(AppUser user)
        {
            var log = userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
            if(log!= null)
            {
                ViewData["credit"] = log.Credit;
                ViewData["img"] = log.Path;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Administration(AppUser user, IFormFile? file)
        {
            var log= userManager.Users.FirstOrDefault(x=>x.UserName==User.Identity!.Name);
            string wwwRootPath = webHostEnvironment.WebRootPath;
            if (file != null)
            {
                 string fileName= Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                 string avatarPath= Path.Combine(wwwRootPath,@"images\Avatar");

                using (var fileStream = new FileStream(Path.Combine(avatarPath, fileName), FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }
                log.Path = @"\images\Avatar\" + fileName;
                ViewData["img"] = log.Path;
                await userManager.UpdateAsync(log);
            }
            //Změna přezdívky s validací
            if (ModelState.IsValid)
            {
                if (log.Credit >= 1000 && log.NickName!=user.NickName && user.NickName!=null && !(user.NickName.Length <=3))
                {
                    log.NickName = user.NickName;
                    log.Credit = log.Credit - 1000;
                    await userManager.UpdateAsync(log);
                }
                AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Nemáte dostatečný kredit na změnu přezdívky nebo máte přezdívku méně jak čtyři znaky" }));
            }
             return RedirectToAction("Administration");
        }

        [HttpGet]
        public IActionResult SendCredit()
        {         
            return View();
        }
        //pro adminy, poslání kreditů pro sebe
        [HttpPost]
        public async Task<IActionResult> SendCredit(AppUser user)
        {
            var log = userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
            if (log != null)
            {
                log.Credit = log.Credit + user.Credit;
                await userManager.UpdateAsync(log);                
            }
            return RedirectToAction("Administration");
        }
        //Načtení uživatelů pro poslání kreditů
        [HttpGet]
        public async Task<IActionResult> ViewSendCredit(string? id)
        {
            if(id is not null)
            {
                var model = new TransferViewModel();
                var userLog = userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
                var  a = await userManager.Users.FirstOrDefaultAsync(c=>c.Id == id);
                model.SourceUserId = userLog.Id;
                model.TargetUserId = a.Id;
                return View(model);
            }
            return View();
        }

        //Validace pro poslání kreditů
        [HttpPost]
        public async Task<IActionResult> ViewSendCredit(TransferViewModel user)
        {
            if(ModelState.IsValid)
            {
                var targetUser = await userManager.FindByIdAsync(user.TargetUserId);
                var sourceUser = await userManager.FindByIdAsync(user.SourceUserId);

                if(sourceUser.Credit >= user.Amount)
                {
                    sourceUser.Credit = sourceUser.Credit - user.Amount;
                    targetUser.Credit = targetUser.Credit + user.Amount;
                    await  userManager.UpdateAsync(sourceUser);
                    await  userManager.UpdateAsync(targetUser);
                    return RedirectToAction("Administration");
                }
            }
            return RedirectToAction("Administration");
        }

        /// <summary>
        /// Pohled uživatelského rozhranní
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AccountView(string? id)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(c=>c.Id==id);
            if(user !=null)
            {
               return View(user);
            }
            return View();
        }


        /// <summary>
        /// Poslání zprávy druhému uživateli
        /// </summary>
        /// <param name="descriptionMessage"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(string descriptionMessage, string message, string? idUser)
        {
            if(idUser==null)
            {
                return RedirectToAction("Administration");
            }
            return View();
        }


        #region Helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }
        
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        #endregion
    }
}
