using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NewPortfolio.Models
{
    public class BuferedImage
    {




        [NotMapped]
        [Display(Name = "Vyberte obrázek")]
        public IFormFile? FormFile { get; set; }
    }
}
