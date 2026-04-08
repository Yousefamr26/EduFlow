using MediatR;

public record GetMaterialsBySessionQuery(int SessionId, string StudentId) : IRequest<IEnumerable<MaterialDto>>;