using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace NewPortfolio.Models
{
    public class RegisterViewModel
    {
        /// <summary>
        /// Email uživatele při registraci
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";


        /// <summary>
        /// Přezdívka uživatele - NickName
        /// </summary>
        [Required]
        [Display(Name = "Přezdívka")]       
        public string NickNameUser { get; set; } = "";       



        /// <summary>
        /// Heslo uživatele
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0} musí mít délku alespoň {2} a nejvíc {1} znaků.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";


        /// <summary>
        /// Potvrezení hesla uživatele
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        [Compare("Password", ErrorMessage = "Zadaná hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; } = "";
        /// <summary>
        /// Potvrzení, že si uživatel přečetl pravidla webové aplikace/fóra
        /// </summary>

        [NotMapped]
        public bool Check { get; set; }
  
    }
}
