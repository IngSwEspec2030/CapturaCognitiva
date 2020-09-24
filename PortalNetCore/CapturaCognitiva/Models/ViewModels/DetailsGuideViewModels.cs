using CapturaCognitiva.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.ViewModels
{
    public class DetailsGuideViewModels
    {
        public int id { get; set; }
        [Display(Name = "Nombre Remitente")]
        public string NameSender { get; set; }
        [Display(Name = "Telefono Remitente")]
        public string CellSender { get; set; }
        [Display(Name = "Ciudad Remitente")]
        public string StateSender { get; set; }
        [Display(Name = "Direccion Remitente")]
        public string AddressSender { get; set; }
        [Display(Name = "Nombre Destinatario")]
        public string NameReceiver { get; set; }
        [Display(Name = "Telefono Destinatario")]
        public string CellReceiver { get; set; }
        [Display(Name = "Ciudad Destinatario")]
        public string StateReceiver { get; set; }
        [Display(Name = "Direccion Destinatario")]
        public string AddressReceiver { get; set; }
        [Display(Name = "Imagen")]
        public string ImageBase64 { get; set; }

    }
}
