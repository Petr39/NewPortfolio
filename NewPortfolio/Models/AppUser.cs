using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace NewPortfolio.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        
        [Display(Name ="Přezdívka")]
        public string? NickName { get; set; }

        [Display(Name ="Váš kredit")]
        public int Credit { get; set; } = 0; 


        //public string? ThumbnailUrl { get; set; } 

        //public IFormFile? Thumnail { get; set; }

        List<Article>? Articles { get; set; }
    }
}
