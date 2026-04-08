using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Repositories
{
    public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(EduDbContext context) : base(context) { }

        public async Task<IEnumerable<Material>> GetBySessionIdAsync(int sessionId)
        {
            return await _dbSet
                .Where(m => m.SessionId == sessionId)
                .ToListAsync();
        }
    }
}
