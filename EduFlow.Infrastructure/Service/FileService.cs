using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class FileService : IFileService
{
    private readonly IHostEnvironment _env;

    public FileService(IHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folderName = "uploads")
    {
        var folder = Path.Combine(_env.ContentRootPath, "wwwroot", folderName);

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var path = Path.Combine(folder, fileName);

        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{folderName}/{fileName}";
    }

    public Stream GetFileStream(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new FileNotFoundException("File URL is null or empty.");

        var relativePath = fileUrl.TrimStart('/');

        var path = Path.Combine(_env.ContentRootPath, "wwwroot", relativePath);

        if (!File.Exists(path))
            throw new FileNotFoundException("File not found.", path);

        return new FileStream(path, FileMode.Open, FileAccess.Read);
    }
}