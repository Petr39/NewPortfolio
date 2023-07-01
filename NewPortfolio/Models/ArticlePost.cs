using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{

    //Přidávání podčlánků
    public class ArticlePost
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Komentář k článku
        /// </summary>
        public string Post { get; set; }

        [ForeignKey("ArticleId")]
        public Article? Article { get; set; }

        public int ArticleId { get; set; }
    }
}
