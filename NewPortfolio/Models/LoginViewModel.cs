using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NewPortfolio.Models
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";

        [Display(Name = "Pamatovat si mě")]
        public bool RememberMe { get; set; }
      
    }
}
