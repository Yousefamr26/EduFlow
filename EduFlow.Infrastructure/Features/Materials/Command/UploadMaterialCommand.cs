using MediatR;
using Microsoft.AspNetCore.Http;

public record UploadMaterialCommand(
    string TeacherId,
    int SessionId,
    IFormFile? File,
    string? VideoUrl,
    string Type
) : IRequest<int>;