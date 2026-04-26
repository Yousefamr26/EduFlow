using EduFlow.Infrastructure.Features.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("admin-dashboard")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAdminDashboard()
    {
        var result = await _mediator.Send(new GetAdminDashboardQuery());
        return Ok(result);
    }

    [HttpGet("teacher-stats/{teacherId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetTeacherStats(string teacherId)
    {
        var result = await _mediator.Send(new GetTeacherStatsQuery(teacherId));
        return Ok(result);
    }

    [HttpGet("my-stats")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetMyStats()
    {
        var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _mediator.Send(new GetTeacherStatsQuery(teacherId!));
        return Ok(result);
    }
}