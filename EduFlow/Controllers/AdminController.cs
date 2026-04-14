using EduFlow.Infrastructure.Features.Auth.Command;
using EduFlow.Infrastructure.Features.WaitingList.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Get waiting list for a specific session
    /// </summary>
    [HttpGet("waiting-list/session/{sessionId}")]
    public async Task<IActionResult> GetSessionWaitingList(int sessionId)
    {
        try
        {
            var query = new GetSessionWaitingListQuery(sessionId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
