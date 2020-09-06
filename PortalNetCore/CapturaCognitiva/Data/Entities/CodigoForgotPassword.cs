using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Data.Entities
{
    public class CodigoForgotPassword
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsUsed { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUso { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
