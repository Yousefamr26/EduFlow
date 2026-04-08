using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Material.Command
{
    public class DeleteMaterialHandler : IRequestHandler<DeleteMaterialCommand, Unit>
    {
        private readonly IUnitOfWork _unit;

        public DeleteMaterialHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Unit> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await _unit.Materials.GetByIdAsync(request.Id);
            if (material == null)
                throw new Exception("Material not found");

           
            // material.IsDeleted = true;
            // material.UpdatedAt = DateTime.UtcNow;
            // _unit.Materials.Update(material);

            _unit.Materials.Delete(material); 
            await _unit.SaveChangesAsync();

            return Unit.Value;
        }
    }
}