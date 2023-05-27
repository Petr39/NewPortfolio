﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class Article
    {

        /// <summary>
        /// Id článku
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Obsah článku
        /// </summary>
        [Required(ErrorMessage ="Vyplňte obsah")]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// Titulek článku
        /// </summary>
        [StringLength(20,ErrorMessage ="Titulek je příliš dlouhý (max 20 znaků)")]
        [Required(ErrorMessage ="Vyplňte titulek")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Popisek článku
        /// </summary>
        [Required(ErrorMessage ="Vyplňte popisek")]
        public string Description { get; set; } = string.Empty;

        public string? AppUserId { get; set; }

        public AppUser? ApplicationUser { get; set; }


        public string? NickName { get; set; }    
    }
}
