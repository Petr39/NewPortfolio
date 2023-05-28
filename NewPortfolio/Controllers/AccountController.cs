using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewPortfolio.Models;

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
            IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
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
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                if (await userManager.FindByEmailAsync(model.Email) is null)
                {
                    //ThumbnailUrl = UploadImage(model.ImageUser)
                    var user = new AppUser { UserName = model.Email, Email = model.Email,NickName=model.NickNameUser };
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


        [Authorize]
        public IActionResult Administration()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Administration(AppUser user)
        {
            var log= userManager.Users.FirstOrDefault(x=>x.UserName==User.Identity!.Name);
                 log.NickName=user.NickName;
                 if(ModelState.IsValid && log.Credit >=100)
                 {
                      log.Credit=log.Credit - 100;
                      await userManager.UpdateAsync(log);
                 }
            return View(user);
        }

        //Musím dodělat obrázek -  problem je v IFormFile pri vytvareni uctu - napsat nahradni tridu pro nacteni do imageformu
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath= Path.Combine(webHostEnvironment.WebRootPath, "ImagesThumb");
            uniqueFileName= new Guid().ToString()+ "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using(FileStream fs= System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
            }

            return uniqueFileName;
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
