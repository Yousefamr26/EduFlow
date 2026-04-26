using EduFlow.Domain.Common;

namespace EduFlow.Domain.Entities
{
    public class Subject : AuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}