using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Features.Subjects.Commands;
using MediatR;

namespace EduFlow.Infrastructure.Features.Subjects.Commands
{
    public class CreateSubjectHandler : IRequestHandler<CreateSubjectCommand, string>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubjectHandler(ISubjectRepository subjectRepository, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            if (await _subjectRepository.IsNameExistsAsync(request.Name))
                return $"Subject '{request.Name}' already exists.";

            var subject = new Subject
            {
                Name = request.Name,
                Description = request.Description
            };

            await _subjectRepository.AddAsync(subject);
            await _unitOfWork.SaveChangesAsync();

            return $"Subject '{request.Name}' created successfully.";
        }
    }
}