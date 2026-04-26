namespace EduFlow.Application.Interfaces.IService
{
    public interface IReportService
    {
        Task<AdminDashboardDto> GetAdminDashboardAsync();
        Task<TeacherStatsDto> GetTeacherStatsAsync(string teacherId);
    }

    public record AdminDashboardDto(
        int TotalStudents,
        int TotalTeachers,
        int TotalSessions,
        int ActiveSubscriptions,
        IEnumerable<TopTeacherDto> TopTeachers
    );

    public record TopTeacherDto(
        string TeacherId,
        string TeacherName,
        double AverageRating,
        int TotalRatings
    );

    public record TeacherStatsDto(
        string TeacherName,
        int TotalSessions,
        int TotalStudents,
        double AverageRating,
        int TotalRatings
    );
}