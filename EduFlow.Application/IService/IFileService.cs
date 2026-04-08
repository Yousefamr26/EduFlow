using Microsoft.AspNetCore.Http;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folderName = "uploads");
    Stream GetFileStream(string fileUrl);

}