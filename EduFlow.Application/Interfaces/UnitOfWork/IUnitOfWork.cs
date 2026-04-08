using EduFlow.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccessCodeRepository AccessCodes { get; }
        ISessionRepository Sessions { get; }
        IBookingRepository Bookings { get; }
        IMaterialRepository Materials { get; }
        INotificationRepository Notifications { get; }
        IAuthRepository Auths { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
