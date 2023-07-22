
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Build
    {
        /// <summary>
        /// Id sestavení postavy
        /// </summary>
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// Popis sestavení postavy
        /// </summary>
        [Required]
        [Display(Name ="Popis buildu")]
        public string Name { get; set; }


        

        [ForeignKey("BuildPostId")]
        public BuildPost? BuildPost { get; set; }    


        /// <summary>
        /// Id sestavení postavy - příspěvku 
        /// </summary>
        public int BuildPostId { get; set; }
    }
}
