
using System.ComponentModel.DataAnnotations;


namespace CapturaCognitiva.Models.AccountViewModels
{
    public class LoginViewModels
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }
    }
}
