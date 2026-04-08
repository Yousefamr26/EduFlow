using MediatR;

public record GetStudentMaterialsQuery(string StudentId, int SessionId) : IRequest<IEnumerable<MaterialDto>>;
