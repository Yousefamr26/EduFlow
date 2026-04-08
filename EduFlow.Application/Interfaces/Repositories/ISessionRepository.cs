using EduFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAvailableSessionsAsync();

        Task<bool> HasConflictAsync(string teacherId, DateTime dateTime);
    }
}
