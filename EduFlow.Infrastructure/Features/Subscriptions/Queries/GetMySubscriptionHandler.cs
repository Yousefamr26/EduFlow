using EduFlow.Application.Interfaces.UnitOfWork;
using MediatR;

namespace EduFlow.Infrastructure.Features.Subscriptions.Queries
{
    public class GetMySubscriptionHandler : IRequestHandler<GetMySubscriptionQuery, SubscriptionDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMySubscriptionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SubscriptionDto?> Handle(GetMySubscriptionQuery request, CancellationToken cancellationToken)
        {
            var sub = await _unitOfWork.Subscriptions.GetActiveSubscriptionAsync(request.StudentId);
            if (sub == null) return null;

            return new SubscriptionDto(
                sub.Package.Name,
                sub.Package.Price,
                sub.StartDate,
                sub.EndDate,
                sub.IsActive
            );
        }
    }
}