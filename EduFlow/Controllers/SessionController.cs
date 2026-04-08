using EduFlow.Infrastructure.Features.Session_Management;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/sessions")]
[Authorize(Roles = "Teacher")]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSessionDto dto)
    {
        var teacherId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(teacherId))
            return Unauthorized("User id not found in token.");

        var command = new CreateSessionCommandWithTeacherId(
            dto.Title,
            dto.Description,
            dto.DateTime,
            dto.Capacity,
            teacherId
        );

        var sessionId = await _mediator.Send(command);

        return Ok(new { SessionId = sessionId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSessionDto dto)
    {
        var teacherId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(teacherId))
            return Unauthorized("User id not found in token.");

        var command = new UpdateSessionCommand(
            id,
            dto.Title,
            dto.Description,
            dto.DateTime,
            dto.Capacity,
            teacherId
        );

        await _mediator.Send(command);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var teacherId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(teacherId))
            return Unauthorized("User id not found in token.");

        var command = new DeleteSessionCommand(id, teacherId);
        await _mediator.Send(command);

        return Ok();
    }
}