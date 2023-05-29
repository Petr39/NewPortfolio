using Microsoft.AspNetCore.Mvc;
using NewPortfolio.Data;
using NewPortfolio.Models;

namespace NewPortfolio.Controllers
{
    public class AvatarController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AvatarController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment) 
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {

            var data=_applicationDbContext.Avatars.ToList();    
            return View(data);
        }

        [HttpGet]
        public IActionResult AddAvatar()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult AddAvatar(Avatar model)
        {

            try
            {

                //if(ModelState.IsValid) 
                //{
                    string uniqueFileName= UploadImage(model);
                    var avatar = new Avatar()
                    {
                        NameAvatar=model.NameAvatar,
                        Path = uniqueFileName
                    };

                    _applicationDbContext.Avatars.Add(avatar);
                    _applicationDbContext.SaveChanges();
                    TempData["Success"] = "Avatar nahrán";
                    return RedirectToAction("Index");
                
                //}
                //ModelState.AddModelError(string.Empty, "Model není validní");

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }




            return View();
        }

        private string UploadImage(Avatar model)
        {
            string uniqueFileName = string.Empty;
            if(model.ImagePath!=null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/Avatar/");
                uniqueFileName = Guid.NewGuid().ToString() + "_"+ model.ImagePath.FileName ;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }




            return uniqueFileName;
        }

    }
}
