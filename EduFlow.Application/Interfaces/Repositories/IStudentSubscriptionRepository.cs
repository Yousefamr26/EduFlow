using EduFlow.Domain.Entities;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface IStudentSubscriptionRepository : IGenericRepository<StudentSubscription>
    {
        Task<StudentSubscription?> GetActiveSubscriptionAsync(string studentId);
        Task<bool> HasActiveSubscriptionAsync(string studentId);
    }
}