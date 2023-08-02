using System.ComponentModel.DataAnnotations;

namespace NewPortfolio.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        public string NameGenre { get; set; }

        public IEnumerable<Article>? Articles { get; set; }

        public IEnumerable<Game>? Games { get; set; }
    }
}
