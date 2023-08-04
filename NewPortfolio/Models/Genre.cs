using System.ComponentModel.DataAnnotations;

namespace NewPortfolio.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Žánr hry")]
        public string NameGenre { get; set; }

        public IEnumerable<Article>? Articles { get; set; }

        public IEnumerable<Game>? Games { get; set; }
    }
}
