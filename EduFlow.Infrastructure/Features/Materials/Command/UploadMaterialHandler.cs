using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Material.Command
{
    public class UploadMaterialHandler : IRequestHandler<UploadMaterialCommand, int>
    {
        private readonly IUnitOfWork _unit;
        private readonly IFileService _fileService;

        public UploadMaterialHandler(IUnitOfWork unit, IFileService fileService)
        {
            _unit = unit;
            _fileService = fileService;
        }

        public async Task<int> Handle(UploadMaterialCommand request, CancellationToken cancellationToken)
        {
            var session = await _unit.Sessions.GetByIdAsync(request.SessionId);

            if (session == null || session.TeacherId != request.TeacherId)
                throw new Exception("Unauthorized");

            string? filePath = null;
            string type = request.Type;

            if (request.File != null)
            {
                filePath = await _fileService.SaveFileAsync(request.File);
                type = "File";
            }

            string? videoUrl = request.VideoUrl;
            if (!string.IsNullOrEmpty(videoUrl))
            {
                type = "Video";
            }

            var material = new Domain.Entities.Material
            {
                SessionId = request.SessionId,
                TeacherId = request.TeacherId,
                FileUrl = filePath,
                VideoUrl = videoUrl,
                Type = type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unit.Materials.AddAsync(material);
            await _unit.SaveChangesAsync();

            return material.Id;
        }
    }
}