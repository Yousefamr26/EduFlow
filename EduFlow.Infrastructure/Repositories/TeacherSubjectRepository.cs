using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Repositories
{
    public class TeacherSubjectRepository : GenericRepository<TeacherSubject>, ITeacherSubjectRepository
    {
        private readonly EduDbContext _context;

        public TeacherSubjectRepository(EduDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsAlreadyAssignedAsync(string teacherId, int subjectId)
            => await _context.TeacherSubjects
                .AnyAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId && !ts.IsDeleted);

        public async Task<IEnumerable<TeacherSubject>> GetByTeacherAsync(string teacherId)
            => await _context.TeacherSubjects
                .Where(ts => ts.TeacherId == teacherId && !ts.IsDeleted)
                .Include(ts => ts.Subject)
                .ToListAsync();
    }
}