using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Data.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Uuid { get; set; }
    }
}
