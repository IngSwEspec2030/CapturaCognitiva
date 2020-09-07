using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.AccountViewModels
{
    public class RegisterViewModel
    {

        [Display(Name = "Id")]
        public string Id { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }
        [Required]
        [Display(Name = "Cedula")]
        public long Cedula { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Rol")]
        public string Rol { get; set; }
    }
}
