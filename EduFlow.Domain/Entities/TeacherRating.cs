using EduFlow.Domain.Common;

namespace EduFlow.Domain.Entities
{
    public class TeacherRating : AuditableEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }

        public int Rating { get; set; } // 1 - 5
        public string? Comment { get; set; }
    }
}