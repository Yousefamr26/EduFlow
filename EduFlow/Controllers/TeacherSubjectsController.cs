using EduFlow.Infrastructure.Features.TeacherSubjects.Commands;
using EduFlow.Infrastructure.Features.TeacherSubjects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/teacher-subjects")]
public class TeacherSubjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherSubjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Assign(AssignTeacherToSubjectCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{teacherId}")]
    [Authorize]
    public async Task<IActionResult> GetSubjectsByTeacher(string teacherId)
    {
        var result = await _mediator.Send(new GetSubjectsByTeacherQuery(teacherId));
        return Ok(result);
    }
}