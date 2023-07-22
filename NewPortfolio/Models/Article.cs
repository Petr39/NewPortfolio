using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Article
    {



        /// <summary>
        /// Id článku
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Obsah článku
        /// </summary>
        [Required(ErrorMessage ="Vyplňte obsah")]
        [Display(Name ="Článek")]
        [StringLength(255, ErrorMessage = "Maximum znaků je 255")]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// Titulek článku
        /// </summary>
      
        [Required(ErrorMessage ="Vyplňte titulek")]
        [Display(Name ="Titulek")]
        [StringLength(12, ErrorMessage ="Maximum znaků je 12")]
        public string? Title { get; set; } = string.Empty;
        /// <summary>
        /// Popisek článku
        /// </summary>
      
        [Required(ErrorMessage ="Vyplňte popisek článku")]
        [Display(Name ="Popis")]
        [StringLength(12, ErrorMessage = "Maximum znaků je 12")]
        public string? Description { get; set; } = string.Empty;


        /// <summary>
        /// Id uživatele
        /// </summary>
        public string? AppUserId { get; set; }


        /// <summary>
        /// Uživatel
        /// </summary>
        public AppUser? ApplicationUser { get; set; }
      
     

        /// <summary>
        /// Cesta k obrázku
        /// </summary>
        public string? ImageUrl { get; set; }   


        /// <summary>
        /// Přezdívka uživatele
        /// </summary>
        public string? NickName { get; set; }   
        
        /// <summary>
        /// Kredity uživatele
        /// </summary>
        public int? Credits { get; set; }


        /// <summary>
        /// Datum registrace uživatele
        /// </summary>
        public string? DateOfRegister { get; set; }


        /// <summary>
        /// Počet příspěvků uživatele
        /// </summary>
        public int? CountPost { get; set; }

            
        public virtual IEnumerable<ArticlePost>? ArticlePosts { get; set; }

        
     
    }
}
