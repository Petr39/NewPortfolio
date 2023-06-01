using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace NewPortfolio.Models
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [Display(Name = "Přezdívka")]       
        public string NickNameUser { get; set; } = "";       

        [Required]
        [StringLength(100, ErrorMessage = "{0} musí mít délku alespoň {2} a nejvíc {1} znaků.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        [Compare("Password", ErrorMessage = "Zadaná hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; } = "";


        [ValidateNever]
        public string? Path { get; set; }



        [NotMapped]
        [Display(Name = "Vyberte obrázek")]
        public IFormFile? ImagePath { get; set; }
    }
}
