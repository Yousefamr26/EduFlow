using EduFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace EduFlow.Domain.Entities
{
    public class Material:BaseEntity
    {
        public int SessionId { get; set; }
        public Session Session { get; set; }

        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }

        public string? FileUrl { get; set; }
        public string? VideoUrl { get; set; }

        public string Type { get; set; }
    }
}
