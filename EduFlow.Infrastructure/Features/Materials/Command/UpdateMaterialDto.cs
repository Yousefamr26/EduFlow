using Microsoft.AspNetCore.Http;

public record UpdateMaterialDto(
    int Id,
    IFormFile? File,
    string? VideoUrl,
    string Type
);