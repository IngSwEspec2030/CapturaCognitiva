
using System.ComponentModel.DataAnnotations;


namespace CapturaCognitiva.Models.AccountViewModels
{
    public class ForgotViewModels
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
