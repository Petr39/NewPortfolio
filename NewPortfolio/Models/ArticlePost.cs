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
        public string? Post { get; set; }

        [ForeignKey("ArticleId")]
        public Article? Article { get; set; }


        [ForeignKey("AppUserId")]
        public  AppUser? AppUser;
        public int ArticleId { get; set; }

        /// <summary>
        /// ID uživatele, který dal komentář k danému článku
        /// </summary>
        public string AppUserId { get; set; }

        /// <summary>
        /// Jméno uživatele, který zadá komentář k článku
        /// </summary>

        public string UserName { get; set; }

        /// <summary>
        /// Datum založení komentáře
        /// </summary>
        public DateTime DateTime { get; set; } 
    }
}
