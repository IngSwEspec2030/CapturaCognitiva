
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Data.Entities
{
    public class imagenestest
    {
        [Key]
        public int iD { get; set; }
        public string Image { get; set; }
    }
}
