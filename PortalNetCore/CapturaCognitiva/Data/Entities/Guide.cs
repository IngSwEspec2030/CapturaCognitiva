using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapturaCognitiva.Data.Entities
{
    public class Guide
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int ImageId { get; set; }
        public virtual Sender Sender { get; set; }
        public virtual Receiver Receiver { get; set; }
        public virtual Image Image { get; set; }
    }
}
