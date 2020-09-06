using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Data.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public int Uaid { get; set; }
        public string ImageBase64 { get; set; }
    }
}
