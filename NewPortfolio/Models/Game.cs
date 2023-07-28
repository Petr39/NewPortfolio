using System.ComponentModel.DataAnnotations;

namespace NewPortfolio.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name ="Název hry")]
        public string GameName { get; set; }


    }
}
