using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Repositories
{
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        private readonly EduDbContext _context;

        public PackageRepository(EduDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Package>> GetActivePackagesAsync()
            => await _context.Packages
                .Where(p => p.IsActive && !p.IsDeleted)
                .ToListAsync();
    }
}