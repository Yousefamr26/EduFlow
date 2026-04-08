using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EduFlow.Infrastructure.Features.Material.Command
{
    public class UpdateMaterialHandler : IRequestHandler<UpdateMaterialCommand, Unit>
    {
        private readonly IUnitOfWork _unit;
        private readonly IFileService _fileService;

        public UpdateMaterialHandler(IUnitOfWork unit, IFileService fileService)
        {
            _unit = unit;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await _unit.Materials.GetByIdAsync(request.Id);
            if (material == null)
                throw new Exception("Material not found");

            string type = material.Type;

            if (request.File != null)
            {
                material.FileUrl = await _fileService.SaveFileAsync(request.File);
                material.VideoUrl = null;
                type = "File";
            }

            if (!string.IsNullOrEmpty(request.VideoUrl))
            {
                material.VideoUrl = request.VideoUrl;
                material.FileUrl = null; 
                type = "Video";
            }

            material.Type = type;
            material.UpdatedAt = DateTime.UtcNow;

            _unit.Materials.Update(material);
            await _unit.SaveChangesAsync();

            return Unit.Value;
        }
    }
}