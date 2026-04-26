using EduFlow.Domain.Common;

namespace EduFlow.Domain.Entities
{
    public class Package : AuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; } // 30 = شهر
        public bool IsActive { get; set; } = true;

        public ICollection<StudentSubscription> Subscriptions { get; set; } = new List<StudentSubscription>();
    }
}