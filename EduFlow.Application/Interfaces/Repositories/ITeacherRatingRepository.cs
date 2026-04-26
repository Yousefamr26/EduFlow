using EduFlow.Domain.Entities;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface ITeacherRatingRepository : IGenericRepository<TeacherRating>
    {
        Task<bool> HasStudentRatedAsync(string studentId, string teacherId);
        Task<IEnumerable<TeacherRating>> GetRatingsByTeacherAsync(string teacherId);
        Task<double> GetAverageRatingAsync(string teacherId);
    }
}