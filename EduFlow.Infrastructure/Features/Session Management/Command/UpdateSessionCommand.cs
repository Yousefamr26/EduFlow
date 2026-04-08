using MediatR;

public class UpdateSessionCommand : IRequest<Unit>
{
    public int SessionId { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime DateTime { get; }
    public int Capacity { get; }
    public string TeacherId { get; } 

    public UpdateSessionCommand(int sessionId, string title, string description, DateTime dateTime, int capacity, string teacherId)
    {
        SessionId = sessionId;
        Title = title;
        Description = description;
        DateTime = dateTime;
        Capacity = capacity;
        TeacherId = teacherId;
    }
}


