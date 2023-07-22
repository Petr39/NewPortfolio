using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Models;

namespace NewPortfolio.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext context;

        public AccountController(UserManager<AppUser> userManager,
               SignInManager<AppUser> signInManager,
               IWebHostEnvironment webHostEnvironment,
               ApplicationDbContext context
              )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            this.context = context;
            
         
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
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
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
                    //if (await userManager.Users.AllAsync(u => u.NickName != model.NickNameUser))
                    if(!NickNameExist(model.NickNameUser))
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

                        AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Přezdívka {model.NickNameUser} je již registrována" }));
                        return View(model);
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
                    if (!NickNameExist(user.NickName))
                    {
                        logUser.NickName = user.NickName;
                        logUser.Credit -=1000;
                        await userManager.UpdateAsync(logUser);
                    }
                    AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Přezdívka je už obsazena" }));
                    return View(logUser);
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
        public async Task<IActionResult> SendCreditForAdmin(AppUser user)
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
                            var message = CreditSendMessage(user.Amount,sourceUser, targetUser);
                            sourceUser.Credit -=  user.Amount;
                            targetUser.Credit +=  user.Amount;
                            await  userManager.UpdateAsync(sourceUser);
                            await  userManager.UpdateAsync(targetUser);
                            await context.Messages.AddAsync(message);
                            await context.SaveChangesAsync();
                            TempData["success"] = $"Kredit uživateli {targetUser.NickName} odeslán";
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
               return View(user);
            TempData["error"] = "Uživatel nenačten";
            return RedirectToAction("Index","Articles");
        }



        //--------------------------- Pomocné metody -------------------------
        
        /// <summary>
        /// Zpráva pro uživatele, že odeslal kredit druhému uživateli
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="userName"></param>
        /// <param name="userNameRecived"></param>
        /// <returns></returns>
        private Message CreditSendMessage(int credit, AppUser userName, AppUser userNameRecived)
        {
                var message = new Message();
            if(userName!=null && userNameRecived != null)
            {
                message.UserId = userName.Id;
                message.MessageHead = userName.NickName;
                message.UserName = userNameRecived.NickName;
               
                message.MessageBody = credit switch
                {
                    > 1 and < 5 => $"Poslány {credit} kredity",
                    > 4 => $"Posláno {credit} kreditů",
                    _   => $"Poslán {credit} kredit"
                };
              return message;
            }
            return message;
        }

        /// <summary>
        /// Ověří, jestli existuje stejná přezdívka
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool NickNameExist(string nickName)
        {
            bool exist = userManager.Users.Any(u => u.NickName == nickName);
            return exist;            
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
