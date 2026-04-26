using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EduFlow.Infrastructure.Features.Ratings.Commands
{
    public class RateTeacherHandler : IRequestHandler<RateTeacherCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public RateTeacherHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<string> Handle(RateTeacherCommand request, CancellationToken cancellationToken)
        {
            // تحقق المدرس موجود
            var teacher = await _userManager.FindByIdAsync(request.TeacherId);
            if (teacher == null)
                return "Teacher not found.";

            var isTeacher = await _userManager.IsInRoleAsync(teacher, "Teacher");
            if (!isTeacher)
                return "User is not a Teacher.";

            // تحقق الطالب مارتش قيّم قبل كده
            var alreadyRated = await _unitOfWork.Ratings.HasStudentRatedAsync(
                request.StudentId, request.TeacherId);
            if (alreadyRated)
                return "You have already rated this teacher.";

            var rating = new TeacherRating
            {
                StudentId = request.StudentId,
                TeacherId = request.TeacherId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            await _unitOfWork.Ratings.AddAsync(rating);
            await _unitOfWork.SaveChangesAsync();

            return "Rating submitted successfully.";
        }
    }
}