using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models.Humans
{
    public class Human 
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [Display(Name ="Jméno")]
        public string Name { get; set; } = string.Empty;



        [Required]
        public int? ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public virtual  Article Article { get; set; }

       
    }
}
