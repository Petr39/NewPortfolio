using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class BuildPost
    {

        /// <summary>
        /// Id příspěvku k sestavení postavy
        /// </summary>
        [Key]
        public int Id {get; set; }


        /// <summary>
        /// Sestavení postavy - název třídy postavy
        /// </summary>
        [Display(Name ="Zadej třídu")]
        public string BuildName { get; set; }

        public virtual IEnumerable<Build>? Builds { get; set; }
    }
}
