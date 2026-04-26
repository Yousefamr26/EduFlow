using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Repositories
{
    public class TeacherRatingRepository : GenericRepository<TeacherRating>, ITeacherRatingRepository
    {
        private readonly EduDbContext _context;

        public TeacherRatingRepository(EduDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasStudentRatedAsync(string studentId, string teacherId)
            => await _context.TeacherRatings
                .AnyAsync(r => r.StudentId == studentId && r.TeacherId == teacherId && !r.IsDeleted);

        public async Task<IEnumerable<TeacherRating>> GetRatingsByTeacherAsync(string teacherId)
            => await _context.TeacherRatings
                .Include(r => r.Student)
                .Where(r => r.TeacherId == teacherId && !r.IsDeleted)
                .ToListAsync();

        public async Task<double> GetAverageRatingAsync(string teacherId)
        {
            var ratings = await _context.TeacherRatings
                .Where(r => r.TeacherId == teacherId && !r.IsDeleted)
                .ToListAsync();

            if (!ratings.Any()) return 0;
            return ratings.Average(r => r.Rating);
        }
    }
}