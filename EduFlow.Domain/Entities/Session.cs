using EduFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Domain.Entities
{
   public class Session:AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public int Capacity { get; set; }
        public int BookedCount { get; set; }

        public bool IsCanceled { get; set; }

        public string SessionCodeHash { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Material> Materials { get; set; }
    }
}
