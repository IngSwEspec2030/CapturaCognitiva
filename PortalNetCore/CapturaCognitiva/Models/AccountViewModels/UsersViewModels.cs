using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Models.AccountViewModels
{
    public class UsersViewModels
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public long Cedula { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public bool IsEnable { get; set; }
    }
}
