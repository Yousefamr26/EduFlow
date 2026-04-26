using EduFlow.Infrastructure.Features.Subjects.Commands;
using EduFlow.Infrastructure.Features.Subjects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/subjects")]
public class SubjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllSubjectsQuery());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateSubjectCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}