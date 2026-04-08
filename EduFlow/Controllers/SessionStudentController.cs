using EduFlow.Infrastructure.Features.Session_Management.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/sessions/discover")]
[Authorize(Roles = "Student")]
public class SessionStudentController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionStudentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailable()
    {
        var result = await _mediator.Send(new GetAvailableSessionsQuery());
        return Ok(result);
    }
}