using EduFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace EduFlow.Domain.Entities
{
    public class Booking:BaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }

        public DateTime BookingTime { get; set; }
    }
}
