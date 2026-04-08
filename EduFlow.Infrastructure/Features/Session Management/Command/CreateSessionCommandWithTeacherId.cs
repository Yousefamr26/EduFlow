using MediatR;

public class CreateSessionCommandWithTeacherId : IRequest<int>
{
    public string Title { get; }
    public string Description { get; }
    public DateTime DateTime { get; }
    public int Capacity { get; }
    public string TeacherId { get; }

    public CreateSessionCommandWithTeacherId(string title, string description, DateTime dateTime, int capacity, string teacherId)
    {
        Title = title;
        Description = description;
        DateTime = dateTime;
        Capacity = capacity;
        TeacherId = teacherId;
    }
}