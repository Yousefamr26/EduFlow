using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public String Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsAccessCodeVerified { get; set; }
        public ICollection<Session> SessionsAsTeacher { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }
}
