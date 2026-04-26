using MediatR;

namespace EduFlow.Infrastructure.Features.Subscriptions.Queries
{
    public record SubscriptionDto(
        string PackageName,
        decimal Price,
        DateTime StartDate,
        DateTime EndDate,
        bool IsActive
    );

    public record GetMySubscriptionQuery(string StudentId) : IRequest<SubscriptionDto?>;
}