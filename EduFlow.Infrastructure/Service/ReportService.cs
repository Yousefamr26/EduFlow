using EduFlow.Application.Interfaces.IService;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Service
{
    public class ReportService : IReportService
    {
        private readonly EduDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportService(EduDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            // Students & Teachers
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");

            // Sessions
            var totalSessions = await _context.Sessions
                .Where(s => !s.IsDeleted && !s.IsCanceled)
                .CountAsync();

            // Active Subscriptions
            var activeSubscriptions = await _context.StudentSubscriptions
                .Where(s => !s.IsDeleted
                    && s.StartDate <= DateTime.UtcNow
                    && s.EndDate >= DateTime.UtcNow)
                .CountAsync();

            // Top Teachers by Rating
            var topTeachers = await _context.TeacherRatings
                .Where(r => !r.IsDeleted)
                .GroupBy(r => r.TeacherId)
                .Select(g => new
                {
                    TeacherId = g.Key,
                    AverageRating = g.Average(r => r.Rating),
                    TotalRatings = g.Count()
                })
                .OrderByDescending(t => t.AverageRating)
                .Take(5)
                .ToListAsync();

            var topTeacherDtos = new List<TopTeacherDto>();
            foreach (var t in topTeachers)
            {
                var teacher = await _userManager.FindByIdAsync(t.TeacherId);
                if (teacher != null)
                {
                    topTeacherDtos.Add(new TopTeacherDto(
                        t.TeacherId,
                        teacher.Name,
                        Math.Round(t.AverageRating, 2),
                        t.TotalRatings
                    ));
                }
            }

            return new AdminDashboardDto(
                students.Count,
                teachers.Count,
                totalSessions,
                activeSubscriptions,
                topTeacherDtos
            );
        }

        public async Task<TeacherStatsDto> GetTeacherStatsAsync(string teacherId)
        {
            var teacher = await _userManager.FindByIdAsync(teacherId);
            if (teacher == null)
                throw new Exception("Teacher not found.");

            // Sessions
            var totalSessions = await _context.Sessions
                .Where(s => s.TeacherId == teacherId && !s.IsDeleted && !s.IsCanceled)
                .CountAsync();

            // Students (unique students booked with this teacher)
            var totalStudents = await _context.Bookings
                .Where(b => b.Session.TeacherId == teacherId && !b.IsDeleted)
                .Select(b => b.StudentId)
                .Distinct()
                .CountAsync();

            // Ratings
            var ratings = await _context.TeacherRatings
                .Where(r => r.TeacherId == teacherId && !r.IsDeleted)
                .ToListAsync();

            var averageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;

            return new TeacherStatsDto(
                teacher.Name,
                totalSessions,
                totalStudents,
                Math.Round(averageRating, 2),
                ratings.Count
            );
        }
    }
}