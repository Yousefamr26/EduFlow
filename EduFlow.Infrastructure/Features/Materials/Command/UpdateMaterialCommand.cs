using MediatR;
using Microsoft.AspNetCore.Http;

public record UpdateMaterialCommand(
    string TeacherId,
    int Id,
    IFormFile? File,
    string? VideoUrl,
    string Type
) : IRequest<Unit>;