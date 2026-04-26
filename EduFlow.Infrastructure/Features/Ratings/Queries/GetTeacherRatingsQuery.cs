using MediatR;

namespace EduFlow.Infrastructure.Features.Ratings.Queries
{
    public record TeacherRatingDto(
        string StudentName,
        int Rating,
        string? Comment,
        DateTime CreatedAt
    );

    public record TeacherRatingsResult(
        double AverageRating,
        int TotalRatings,
        IEnumerable<TeacherRatingDto> Ratings
    );

    public record GetTeacherRatingsQuery(string TeacherId) : IRequest<TeacherRatingsResult>;
}