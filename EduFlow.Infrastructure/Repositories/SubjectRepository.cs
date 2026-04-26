using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly EduDbContext _context;

        public SubjectRepository(EduDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsNameExistsAsync(string name)
            => await _context.Subjects.AnyAsync(s => s.Name == name && !s.IsDeleted);

        public async Task<IEnumerable<Subject>> GetSubjectsByTeacherAsync(string teacherId)
            => await _context.Subjects
                .Where(s => s.TeacherSubjects.Any(ts => ts.TeacherId == teacherId) && !s.IsDeleted)
                .ToListAsync();
    }
}