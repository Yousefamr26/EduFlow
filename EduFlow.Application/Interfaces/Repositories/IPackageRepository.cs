using EduFlow.Domain.Entities;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        Task<IEnumerable<Package>> GetActivePackagesAsync();
    }
}