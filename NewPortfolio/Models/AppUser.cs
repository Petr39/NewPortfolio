﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPortfolio.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        
        [Display(Name ="Přezdívka")]
        public string? NickName { get; set; }

        [Display(Name ="Váš kredit")]
        public int Credit { get; set; }

        
        public string? Path { get; set; }


        [NotMapped]
        public IFormFile? ImagePath { get; set; }

        List<Article>? Articles { get; set; }

        public DateTime DateOfRegister { get; set; } = DateTime.Now;

        public int CountPost { get; set; } = 0;
    }
}
