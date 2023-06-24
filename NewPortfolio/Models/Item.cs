using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Název věci")]
        public string NameItem { get; set; }

        
        public string? PathItem { get; set; }

        [NotMapped]
        [Display(Name ="Vyberte obrázek")]
        private  IFormFile? FormFile { get; set; }

      

    }
}
