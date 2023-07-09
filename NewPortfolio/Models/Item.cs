using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Název věci
        /// </summary>
        [Required]
        [Display(Name ="Název věci")]
        public string NameItem { get; set; }
        /// <summary>
        /// Popis předmětu
        /// </summary>
        [Required]
        [Display(Name ="Popis předmětu")]
        public string? DescriptionItem { get; set; }

        public string? PathItem { get; set; }

        [NotMapped]
        [Display(Name ="Vyberte obrázek")]
        private  IFormFile? FormFile { get; set; }

      

    }
}
