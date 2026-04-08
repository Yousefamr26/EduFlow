using MediatR;

public class CreateSessionCommand : IRequest<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public int Capacity { get; set; }

    
 
}