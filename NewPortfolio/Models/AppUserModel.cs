using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NewPortfolio.Models
{
    public class AppUserModel
    {


        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        [Display(Name = "Přezdívka")]
        public string? NickName { get; set; }

        [Display(Name = "Váš kredit")]
        public int Credit { get; set; } = 0;


        public string? CoverImageUrl { get; set; }

        //public IFormFile? Thumnail { get; set; }

        List<Article>? Articles { get; set; }
    }
}
