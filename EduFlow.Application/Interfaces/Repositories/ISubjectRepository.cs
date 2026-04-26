using EduFlow.Domain.Entities;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<bool> IsNameExistsAsync(string name);
        Task<IEnumerable<Subject>> GetSubjectsByTeacherAsync(string teacherId);
    }
}