using EduFlow.Infrastructure.Features.Packages.Commands;
using EduFlow.Infrastructure.Features.Subscriptions.Commands;
using EduFlow.Infrastructure.Features.Subscriptions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/packages")]
public class PackagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PackagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreatePackageCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}