using EduFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface IAccessCodeRepository : IGenericRepository<AccessCodes>
    {
        Task<AccessCodes?> GetByCodeHashAsync(string codeHash);
        Task<AccessCodes?> GetByUserIdAsync(string userId);
    }
}
