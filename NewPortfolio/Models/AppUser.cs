using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Jméno uživatele
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Příjmení uživatele
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Přeszdívka uživatele
        /// </summary>
        [Display(Name ="Přezdívka")]
        [StringLength(8)]
        public string? NickName { get; set; }

        /// <summary>
        /// Počet kreditů do začátku je defaultně 1000
        /// </summary>
        [Display(Name = "Váš kredit")]
        public int Credit { get; set; } = 1000;

        /// <summary>
        /// Cesta k obrázku avatara
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Cesta k obrázku avatara - max velikost 40kB
        /// </summary>
        [NotMapped]
        public IFormFile? ImagePath { get; set; }


        /// <summary>
        /// Článek
        /// </summary>
        List<Article>? Articles { get; set; }

        /// <summary>
        /// Datum registrace uživatele
        /// </summary>
        public DateTime DateOfRegister { get; set; } = DateTime.Now;
        /// <summary>
        /// Počítadlo příspěvků uživatele
        /// </summary>
        public int CountPost { get; set; } = 0;

        
        /// <summary>
        /// Zprávy od uživatelů
        /// </summary>
        public virtual IEnumerable<Message>? Messages { get; set; }

        
    }

    

   
}
