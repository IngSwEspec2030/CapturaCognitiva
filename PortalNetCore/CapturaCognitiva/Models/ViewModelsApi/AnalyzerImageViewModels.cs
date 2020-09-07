using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.ViewModelsApi
{
    public class AnalyzerImageViewModels
    {
        [Required]
        public string UserToken { get; set; }
        [Required]
        public string ImageBase64 { get; set; }
    }
}
