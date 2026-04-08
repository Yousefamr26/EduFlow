using EduFlow.Infrastructure.Features.Dashboard.Queries.Teacher;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("student/upcoming")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Upcoming()
    {
        var studentId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (string.IsNullOrEmpty(studentId))
            return Unauthorized("StudentId not found in token");

        return Ok(await _mediator.Send(new GetStudentUpcomingSessionsQuery(studentId)));
    }

    [HttpGet("teacher/sessions")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> MySessions()
    {
        var teacherId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(teacherId))
            return Unauthorized("TeacherId not found in token");

        return Ok(await _mediator.Send(new GetTeacherSessionsQuery(teacherId)));
    }

    [HttpGet("teacher/students/{sessionId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Students(int sessionId)
    {
        return Ok(await _mediator.Send(new GetSessionStudentsQuery(sessionId)));
    }
}