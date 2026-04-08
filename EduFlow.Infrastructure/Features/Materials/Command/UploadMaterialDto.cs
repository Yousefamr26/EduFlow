using Microsoft.AspNetCore.Http;

public record UploadMaterialDto(
    int SessionId,
    IFormFile? File,
    string? VideoUrl,
    string Type
);