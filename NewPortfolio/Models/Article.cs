using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Article
    {
        [Key]
        /// <summary>
        /// Id článku
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Obsah článku
        /// </summary>
        [Required(ErrorMessage ="Vyplňte obsah")]
        [Display(Name ="Článek")]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// Titulek článku
        /// </summary>
      
        [Required(ErrorMessage ="Vyplňte titulek")]
        [Display(Name ="Titulek")]
        public string? Title { get; set; } = string.Empty;
        /// <summary>
        /// Popisek článku
        /// </summary>
      
        [Required(ErrorMessage ="Vyplňte popisek článku")]
        [Display(Name ="Popis")]
        public string? Description { get; set; } = string.Empty;

        public string? AppUserId { get; set; }



        public AppUser? ApplicationUser { get; set; }
      
     


        public string? ImageUrl { get; set; }   

        public string? NickName { get; set; }   
        

        public int? Credits { get; set; }

        public string? DateOfRegister { get; set; }

        public int? CountPost { get; set; }
    }
}
