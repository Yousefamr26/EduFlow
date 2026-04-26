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
        IWaitingListRepository WaitingList { get; }
        IRoomRepository Rooms { get; }   
        ISubjectRepository Subjects { get; }
        ITeacherSubjectRepository TeacherSubjects { get; }
        IPackageRepository Packages { get; }
        IStudentSubscriptionRepository Subscriptions { get; }
        ITeacherRatingRepository Ratings { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
