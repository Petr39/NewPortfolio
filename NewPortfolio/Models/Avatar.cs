using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Avatar
    {

        [Key]
        public int Id { get; set; }

       
        public string NameAvatar { get; set; }  
        public string Path { get; set; }



        [NotMapped]
        [Display(Name ="Vyberte obrázek")]
        public IFormFile ImagePath { get; set; }

    }
}
