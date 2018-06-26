using System.ComponentModel.DataAnnotations;

namespace NM_Catalog.ViewModels
{
    public class CustomerRegisterViewModel
    {
        public int Id { get; set; }  
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string RepeatedPassword { get; set; }
    }
}