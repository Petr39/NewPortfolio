
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Název hry")]
        public string GameName { get; set; }

     

        public virtual IEnumerable<Article>? Articles { get; set; }

        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }

        public int? GenreId { get; set; }


        public virtual IEnumerable<Item>? Items { get; set; }    
    }
}
