using EduFlow.Application.Interfaces.Repositories;
using MediatR;

namespace EduFlow.Infrastructure.Features.TeacherSubjects.Queries
{
    public class GetSubjectsByTeacherHandler : IRequestHandler<GetSubjectsByTeacherQuery, IEnumerable<TeacherSubjectDto>>
    {
        private readonly ISubjectRepository _subjectRepository;

        public GetSubjectsByTeacherHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<TeacherSubjectDto>> Handle(GetSubjectsByTeacherQuery request, CancellationToken cancellationToken)
        {
            var subjects = await _subjectRepository.GetSubjectsByTeacherAsync(request.TeacherId);

            return subjects.Select(s => new TeacherSubjectDto(s.Id, s.Name, s.Description));
        }
    }
}