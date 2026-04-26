using EduFlow.Domain.Common;

namespace EduFlow.Domain.Entities
{
    public class StudentSubscription : AuditableEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    }
}