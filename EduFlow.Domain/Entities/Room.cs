using EduFlow.Domain.Common;

namespace EduFlow.Domain.Entities
{
    public class Room : AuditableEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string? Location { get; set; }
        public bool IsAvailable { get; set; } = true;

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}