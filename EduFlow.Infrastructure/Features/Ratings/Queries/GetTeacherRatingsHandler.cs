using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;

namespace EduFlow.Infrastructure.Features.Ratings.Queries
{
    public class GetTeacherRatingsHandler : IRequestHandler<GetTeacherRatingsQuery, TeacherRatingsResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTeacherRatingsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TeacherRatingsResult> Handle(GetTeacherRatingsQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.Ratings.GetRatingsByTeacherAsync(request.TeacherId);
            var average = await _unitOfWork.Ratings.GetAverageRatingAsync(request.TeacherId);

            var dtos = ratings.Select(r => new TeacherRatingDto(
                r.Student.Name,
                r.Rating,
                r.Comment,
                r.CreatedAt
            ));

            return new TeacherRatingsResult(
                Math.Round(average, 2),
                ratings.Count(),
                dtos
            );
        }
    }
}