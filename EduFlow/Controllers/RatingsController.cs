using EduFlow.Infrastructure.Features.Ratings.Commands;
using EduFlow.Infrastructure.Features.Ratings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/ratings")]
public class RatingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Rate(RateTeacherCommand command)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(command with { StudentId = studentId! });
        return Ok(result);
    }

    [HttpGet("{teacherId}")]
    [Authorize]
    public async Task<IActionResult> GetTeacherRatings(string teacherId)
    {
        var result = await _mediator.Send(new GetTeacherRatingsQuery(teacherId));
        return Ok(result);
    }
}