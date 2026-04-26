using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using MediatR;

namespace EduFlow.Infrastructure.Features.Subscriptions.Commands
{
    public class SubscribeHandler : IRequestHandler<SubscribeCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscribeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            // تحقق مش عنده subscription فعالة
            var hasActive = await _unitOfWork.Subscriptions.HasActiveSubscriptionAsync(request.StudentId);
            if (hasActive)
                return "Student already has an active subscription.";

            // تحقق الباقة موجودة
            var package = await _unitOfWork.Packages.GetByIdAsync(request.PackageId);
            if (package == null || !package.IsActive)
                return "Package not found or inactive.";

            var subscription = new StudentSubscription
            {
                StudentId = request.StudentId,
                PackageId = request.PackageId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(package.DurationInDays)
            };

            await _unitOfWork.Subscriptions.AddAsync(subscription);
            await _unitOfWork.SaveChangesAsync();

            return $"Subscribed successfully until {subscription.EndDate:yyyy-MM-dd}.";
        }
    }
}