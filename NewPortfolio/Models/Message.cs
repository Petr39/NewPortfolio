using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{

    
    public class Message
    {
        /// <summary>
        /// Id zprávy pro uživatele
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Hlavička zprávy pro uživatele
        /// </summary>
        [Display(Name ="Hlavička")]
        [Required]
        [StringLength(12, ErrorMessage = "Zadaný počet znaků může být minimálně 2 a maximálně 12",MinimumLength =2)]
        public string MessageHead { get; set; }

        /// <summary>
        /// Zpráva pro uživatele
        /// </summary>
        [Display(Name ="Zpráva")]
        [Required]
        [StringLength(255, ErrorMessage = "Zadaný počet znaků může být minimálně 2 a maximálně 12", MinimumLength = 2)]
       
        public string MessageBody { get; set; }

        /// <summary>
        /// Datum vložení zprávy
        /// </summary>
        public DateTime DateTime { get; set; } 

        /// <summary>
        /// Uživatel, na kterého je vázaná zpráva
        /// </summary>
        [ForeignKey("UserId")]
        [NotMapped]
        public AppUser? User { get; set; }

        /// <summary>
        /// Přezdívka uživatele
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Id uživatele
        /// </summary>
        public string? UserId { get; set; }
    }
}
