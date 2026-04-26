using EduFlow.Application.Interfaces.IService;
using MediatR;

namespace EduFlow.Infrastructure.Features.Reports.Queries
{
    public class GetTeacherStatsHandler : IRequestHandler<GetTeacherStatsQuery, TeacherStatsDto>
    {
        private readonly IReportService _reportService;

        public GetTeacherStatsHandler(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<TeacherStatsDto> Handle(GetTeacherStatsQuery request, CancellationToken cancellationToken)
            => await _reportService.GetTeacherStatsAsync(request.TeacherId);
    }
}