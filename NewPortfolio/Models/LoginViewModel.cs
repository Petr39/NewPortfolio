using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NewPortfolio.Models
{
    public class LoginViewModel
    {

        /// <summary>
        /// Email uživatele
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        public string Email { get; set; } = "";


        /// <summary>
        /// Heslo uživatele
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";


        /// <summary>
        /// Zapamatování uživatele
        /// </summary>
        [Display(Name = "Pamatovat si mě")]
        public bool RememberMe { get; set; }
      
    }
}
