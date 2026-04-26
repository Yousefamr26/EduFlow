using EduFlow.Domain.Entities;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface ITeacherSubjectRepository : IGenericRepository<TeacherSubject>
    {
        Task<bool> IsAlreadyAssignedAsync(string teacherId, int subjectId);
        Task<IEnumerable<TeacherSubject>> GetByTeacherAsync(string teacherId);
    }
}