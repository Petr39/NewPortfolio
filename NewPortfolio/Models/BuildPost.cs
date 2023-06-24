using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class BuildPost
    {
        [Key]
        public int Id {get; set; }

        [Display(Name ="Zadej třídu")]
        public string BuildName { get; set; }

        public virtual IEnumerable<Build>? Builds { get; set; }
    }
}
