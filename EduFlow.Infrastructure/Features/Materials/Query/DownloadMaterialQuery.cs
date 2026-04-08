using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public record DownloadMaterialQuery(int MaterialId, string StudentId) : IRequest<FileStreamResult?>;

public class DownloadMaterialHandler : IRequestHandler<DownloadMaterialQuery, FileStreamResult?>
{
    private readonly IUnitOfWork _unit;
    private readonly IFileService _fileService;

    public DownloadMaterialHandler(IUnitOfWork unit, IFileService fileService)
    {
        _unit = unit;
        _fileService = fileService;
    }

    public async Task<FileStreamResult?> Handle(DownloadMaterialQuery request, CancellationToken cancellationToken)
    {
        var material = await _unit.Materials.GetByIdAsync(request.MaterialId);
        if (material == null)
            throw new Exception("Material not found");

        // Check if student booked the session
        var isBooked = await _unit.Bookings.IsAlreadyBookedAsync(request.StudentId, material.SessionId);
        if (!isBooked)
            throw new Exception("Unauthorized");

        if (material.FileUrl == null)
            return null;

        var stream = _fileService.GetFileStream(material.FileUrl); 
        var contentType = "application/octet-stream"; 
        return new FileStreamResult(stream, contentType)
        {
            FileDownloadName = Path.GetFileName(material.FileUrl)
        };
    }
}