using EduFlow.Application.Interfaces.IService;
using MediatR;

namespace EduFlow.Infrastructure.Features.Reports.Queries
{
    public record GetAdminDashboardQuery() : IRequest<AdminDashboardDto>;
}