using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Data.Entities
{
    public class Sender
    {
        [Key]
        public int Id { get; set; }      
        public string Name { get; set; }      
        public string Address { get; set; }   
        public string State { get; set; }       
        public string Cell { get; set; }
    }
}
