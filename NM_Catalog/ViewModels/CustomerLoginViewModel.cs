﻿using System.ComponentModel.DataAnnotations;

namespace NM_Catalog.ViewModels
{
    public class CustomerLoginViewModel
    {
        public int Id { get; set; }  
                
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}