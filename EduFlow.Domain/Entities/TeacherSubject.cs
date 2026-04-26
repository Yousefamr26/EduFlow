using EduFlow.Domain.Common;

namespace EduFlow.Domain.Entities
{
    public class TeacherSubject : BaseEntity
    {
        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}