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
    public class AccessCodeRepository : GenericRepository<AccessCodes>, IAccessCodeRepository
    {
        public AccessCodeRepository(EduDbContext context) : base(context) { }

        public async Task<AccessCodes?> GetByCodeHashAsync(string codeHash)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.CodeHash == codeHash);
        }
        public async Task<AccessCodes?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ExpiryDate) // يجيب أحدث كود
                .FirstOrDefaultAsync();
        }
    }
}
