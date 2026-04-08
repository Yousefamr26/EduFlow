using EduFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Domain.Entities
{
    public class AccessCodes:BaseEntity
    {
        public string CodeHash { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Attempts { get; set; }
    }
}
