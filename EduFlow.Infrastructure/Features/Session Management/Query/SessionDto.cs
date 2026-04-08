using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Query
{
    public class SessionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int Capacity { get; set; }
        public int BookedCount { get; set; }
        public string TeacherName { get; set; }
    }
}
