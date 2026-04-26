using EduFlow.Application.Interfaces.IService;
using MediatR;

namespace EduFlow.Infrastructure.Features.Reports.Queries
{
    public class GetAdminDashboardHandler : IRequestHandler<GetAdminDashboardQuery, AdminDashboardDto>
    {
        private readonly IReportService _reportService;

        public GetAdminDashboardHandler(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<AdminDashboardDto> Handle(GetAdminDashboardQuery request, CancellationToken cancellationToken)
            => await _reportService.GetAdminDashboardAsync();
    }
}