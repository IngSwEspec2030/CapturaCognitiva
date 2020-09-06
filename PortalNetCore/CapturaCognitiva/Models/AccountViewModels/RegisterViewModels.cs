using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.AccountViewModels
{
    public class EditViewModels
    {

        [Display(Name = "Id")]
        public string Id { get; set; }     
        [Required]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }
        [Required]
        [Display(Name = "Cedula")]
        public long Cedula { get; set; }
        [Required]
        [Display(Name = "Rol")]
        public string Rol { get; set; }
    }
}
