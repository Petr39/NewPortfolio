
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class CreatePostVM
    {
        /// <summary>
        /// Id článku
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Obsah článku
        /// </summary>
        [Required(ErrorMessage = "Vyplňte obsah")]
        [Display(Name = "Článek")]
        [StringLength(255, ErrorMessage = "Maximum znaků je 255")]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// Titulek článku
        /// </summary>
        [StringLength(8, ErrorMessage = "Titulek je příliš dlouhý (max 8 znaků)")]
        [Required(ErrorMessage = "Vyplňte titulek")]
        [Display(Name = "Titulek")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Popisek článku
        /// </summary>
        [Required(ErrorMessage = "Vyplňte popisek")]
        [Display(Name = "Popis")]
        [StringLength(8, ErrorMessage = "Titulek je příliš dlouhý (max 8 znaků)")]
        public string Description { get; set; } = string.Empty;

       /// <summary>
       /// Id uživatele
       /// </summary>
        public string? AppUserId { get; set; }

        /// <summary>
        /// Přezdívka uživatele/ NickName
        /// </summary>
        public string? NickName { get; set; }

        public string? ImageUrlVM { get; set; }


        /// <summary>
        /// Cesta k obrázku/avatara uživatele max 40 kB
        /// </summary>
        [NotMapped]
        public IFormFile? Image { get; set; }


        /// <summary>
        /// Počet příspěvků uživatele
        /// </summary>
        public int CountPost { get; set; }



        /// <summary>
        /// Id třídy postavy 
        /// </summary>
        [ForeignKey("BuildPost")]
        [Display(Name = "Vyberte svoji třídu")]

        public int BuildPostId { get; set; }

        [Display(Name = "Vyberte svoji třídu")]
        public virtual BuildPost? BuildPost { get; set; }

        [Display(Name ="Vyberte hru")]
        public virtual IEnumerable<Game>? Games { get; set; } 


        public int GameId { get; set; }

    }
}
