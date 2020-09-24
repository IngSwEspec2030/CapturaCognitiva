using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.ViewModels
{
    public class SummaryViewModels
    {

        public int Id { get; set; }
        [Display(Name = "Uuid")]
        public string Uuid { get; set; }
        [Display(Name = "Extraccion completa")]
        public bool IsComplete { get; set; }
        [Display(Name = "Esta cargado")]
        public bool IsUpload { get; set; }      
        [Display(Name = "Fecha Carga")]
        public DateTime? FechaCarga { get; set; }

    }
}
