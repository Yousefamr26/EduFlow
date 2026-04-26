using EduFlow.Application.Interfaces.IService;
using MediatR;

namespace EduFlow.Infrastructure.Features.Reports.Queries
{
    public record GetTeacherStatsQuery(string TeacherId) : IRequest<TeacherStatsDto>;
}