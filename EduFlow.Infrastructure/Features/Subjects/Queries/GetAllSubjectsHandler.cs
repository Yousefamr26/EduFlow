using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Infrastructure.Features.Subjects.Queries;
using MediatR;

namespace EduFlow.Infrastructure.Features.Subjects.Queries
{
    public class GetAllSubjectsHandler : IRequestHandler<GetAllSubjectsQuery, IEnumerable<SubjectDto>>
    {
        private readonly ISubjectRepository _subjectRepository;

        public GetAllSubjectsHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<SubjectDto>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
        {
            var subjects = await _subjectRepository.GetAllAsync();

            return subjects
                .Where(s => !s.IsDeleted)
                .Select(s => new SubjectDto(s.Id, s.Name, s.Description));
        }
    }
}