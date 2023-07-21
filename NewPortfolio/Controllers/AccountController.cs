using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;

using NewPortfolio.Models;
using NewPortfolio.ModelView;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.IO.Compression;


namespace NewPortfolio.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;
    
       

        public AccountController(UserManager<AppUser> userManager,
               SignInManager<AppUser> signInManager,
               IWebHostEnvironment webHostEnvironment
              )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            
         
        }
        /// <summary>
        /// Přihlášení uživatele
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    
        /// <summary>
        /// Přihlášení uživatele z databaáze
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl =null)
        {
            ViewData["ReturnUrl"] = returnUrl;


            if (!ModelState.IsValid)
                return View(model);

           //Přihlásí uživatele
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

        /// <summary>
        /// Odhlášení uživatele
        /// </summary>
        /// <returns></returns>
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
       
        /// <summary>
        /// Registrace uživatele
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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
                
                 var result = await userManager.CreateAsync(user, model.Password);

                

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

        /// <summary>
        /// Administrace účtu
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Administration()
        {
            var logUser = userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
            if(logUser!= null)
            {
                ViewData["credit"] = logUser.Credit;
                ViewData["img"] = logUser.Path;
               //Dopsat kód
            }
              
            return View();
        }

        /// <summary>
        /// Změny v administraci účtu, přezdívka, avatar
        /// </summary>
        /// <param name="user"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Administration(AppUser user, IFormFile? file)
        {
            var logUser= userManager.Users.FirstOrDefault(x=>x.UserName==User.Identity!.Name);
            string wwwRootPath = webHostEnvironment.WebRootPath;
            
            //Změna přezdívky s validací a avatar
            if (ModelState.IsValid)
            {

                //Odeslání kreditů , při změně přezdívky
                if (logUser.Credit >= 1000 && logUser.NickName!=user.NickName && user.NickName!=null && !(user.NickName.Length <=3))
                {
                    logUser.NickName = user.NickName;
                    logUser.Credit -=1000;
                    await userManager.UpdateAsync(logUser);
                   // TempData["info"] = $"Odesláno {1000} kreditů za změnu přezdívky";
                }
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string avatarPath = Path.Combine(wwwRootPath, @"images\Avatar");

                    using (var fileStream = new FileStream(Path.Combine(avatarPath, fileName), FileMode.Create))
                    {
                        file.CopyToAsync(fileStream);
                    }
                    logUser.Path = @"\images\Avatar\" + fileName;
                    ViewData["img"] = logUser.Path;
                    await userManager.UpdateAsync(logUser);
                
                    //TempData["info"] = $"Proběhla změna avatara";
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

        //pro adminy, poslání kreditů pro sebe, testovací!!!
        [HttpPost]
        public async Task<IActionResult> SendCredit(AppUser user)
        {
            var logUser = userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
            if (logUser != null)
            {
                logUser.Credit +=  user.Credit;
                await userManager.UpdateAsync(logUser);                
            }
            return RedirectToAction("Administration");
        }
        /// <summary>
        /// Načtení uživatelů pro poslání kreditů
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewSendCredit(string? id)
        {
            if(id != null)
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

        /// <summary>
        /// Validace pro odeslání kreditů
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ViewSendCredit(TransferViewModel user)
        {
            if(ModelState.IsValid)
            {
                try
                {
                     var targetUser = await userManager.FindByIdAsync(user.TargetUserId);
                     var sourceUser = await userManager.FindByIdAsync(user.SourceUserId);
                        if(sourceUser.Credit >= user.Amount)
                        {
                            sourceUser.Credit -=  user.Amount;
                            targetUser.Credit +=  user.Amount;
                            await  userManager.UpdateAsync(sourceUser);
                            await  userManager.UpdateAsync(targetUser);
                            return RedirectToAction("Administration");
                        }

                    return RedirectToAction("Administration");
                  
                }
                catch (Exception)
                {
                    
                    throw;
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

        //--------------------------- Pomocné metody -------------------------
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
