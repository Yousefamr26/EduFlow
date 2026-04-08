using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.BookingSystem.Queries
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
