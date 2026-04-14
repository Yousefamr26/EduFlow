namespace EduFlow.Infrastructure.Features.WaitingList.Queries
{
    public class WaitingListEntryDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public DateTime RequestTime { get; set; }
        public int QueuePosition { get; set; }
    }

    public class SessionWaitingListDto
    {
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public DateTime SessionDateTime { get; set; }
        public int TotalWaiting { get; set; }
        public List<WaitingListEntryDto> Entries { get; set; } = new();
    }

    public class StudentWaitingListPositionDto
    {
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public DateTime SessionDateTime { get; set; }
        public int QueuePosition { get; set; }
        public int TotalInQueue { get; set; }
    }
}
