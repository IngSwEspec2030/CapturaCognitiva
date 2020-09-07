using CapturaCognitiva.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.ViewModels
{
    public class ImageFormViewModels
    {
        public int Id { get; set; }
        public string ImageBase64 { get; set; }
        [Required]
        [Display(Name = "Nombres")]
        public string NameSender { get; set; }
        [Required]
        [Display(Name = "Direccion")]
        public string AddressSender { get; set; }
        [Required]
        [Display(Name = "Ciudad")]
        public string StateSender { get; set; }
        [Required]
        [Display(Name = "Telefono")]
        public string CellSender { get; set; }
        [Required]
        [Display(Name = "Nombres")]
        public string NameReceiver { get; set; }
        [Required]
        [Display(Name = "Direccion")]
        public string AddressReceiver { get; set; }
        [Required]
        [Display(Name = "Ciudad")]
        public string StateReceiver { get; set; }
        [Required]
        [Display(Name = "Telefono")]
        public string CellReceiver { get; set; }
    }
}
