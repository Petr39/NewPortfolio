
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Build
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [ForeignKey("BuildPostId")]
        public BuildPost? BuildPost { get; set; }    

        public int BuildPostId { get; set; }
    }
}
