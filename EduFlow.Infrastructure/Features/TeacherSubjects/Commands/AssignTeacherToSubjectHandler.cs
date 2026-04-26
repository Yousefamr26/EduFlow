using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EduFlow.Infrastructure.Features.TeacherSubjects.Commands
{
    public class AssignTeacherToSubjectHandler : IRequestHandler<AssignTeacherToSubjectCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignTeacherToSubjectHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<string> Handle(AssignTeacherToSubjectCommand request, CancellationToken cancellationToken)
        {
            // تحقق إن اليوزر موجود وهو Teacher
            var teacher = await _userManager.FindByIdAsync(request.TeacherId);
            if (teacher == null)
                return "Teacher not found.";

            var isTeacher = await _userManager.IsInRoleAsync(teacher, "Teacher");
            if (!isTeacher)
                return "User is not a Teacher.";

            // تحقق إن المادة موجودة
            var subject = await _unitOfWork.Subjects.GetByIdAsync(request.SubjectId);
            if (subject == null)
                return "Subject not found.";

            // تحقق مش مربوط قبل كده
            var existing = await _unitOfWork.TeacherSubjects.IsAlreadyAssignedAsync(
                request.TeacherId, request.SubjectId);
            if (existing)
                return "Teacher is already assigned to this subject.";

            var teacherSubject = new TeacherSubject
            {
                TeacherId = request.TeacherId,
                SubjectId = request.SubjectId
            };

            await _unitOfWork.TeacherSubjects.AddAsync(teacherSubject);
            await _unitOfWork.SaveChangesAsync();

            return "Teacher assigned to subject successfully.";
        }
    }
}