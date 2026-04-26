using EduFlow.Infrastructure.Features.Subscriptions.Commands;
using EduFlow.Infrastructure.Features.Subscriptions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Subscribe(SubscribeCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMySubscription()
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new GetMySubscriptionQuery(studentId!));
        if (result == null) return NotFound("No active subscription.");
        return Ok(result);
    }
}