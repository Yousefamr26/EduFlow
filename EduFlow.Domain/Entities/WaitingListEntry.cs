using EduFlow.Domain.Common;

namespace EduFlow.Domain.Entities
{
    public class WaitingListEntry : BaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }

        public DateTime RequestTime { get; set; }

        public int QueuePosition { get; set; }
    }
}
