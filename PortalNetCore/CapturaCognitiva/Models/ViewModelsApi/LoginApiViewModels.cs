using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapturaCognitiva.Models.ViewModelsApi
{
    public class LoginApiViewModels
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }       
    }
}