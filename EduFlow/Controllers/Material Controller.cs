using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Infrastructure.Features.Material.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/materials")]
[Authorize]
public class MaterialController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unit;
    private readonly IFileService _fileService;

    public MaterialController(IMediator mediator, IUnitOfWork unit, IFileService fileService)
    {
        _mediator = mediator;
        _unit = unit;
        _fileService = fileService;
    }

    private string GetTeacherIdFromToken()
    {
        var teacherId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(teacherId))
            throw new UnauthorizedAccessException("Teacher id not found in token.");
        return teacherId;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Upload([FromForm] UploadMaterialDto dto)
    {
        try
        {
            var teacherId = GetTeacherIdFromToken();

            var command = new UploadMaterialCommand(
                teacherId,
                dto.SessionId,
                dto.File,
                dto.VideoUrl,
                dto.Type
            );

            var result = await _mediator.Send(command);

            return Ok(new { MaterialId = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

   
    [HttpPut]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Update([FromForm] UpdateMaterialDto dto)
    {
        try
        {
            var teacherId = GetTeacherIdFromToken();

            var command = new UpdateMaterialCommand(
                teacherId,
                dto.Id,
                dto.File,
                dto.VideoUrl,
                dto.Type
            );

            await _mediator.Send(command);

            return Ok();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteMaterialCommand(id);
            await _mediator.Send(command);
            return Ok(new { Message = "Material deleted successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet("{sessionId}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMaterials(int sessionId)
    {
        var studentId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(studentId))
            return Unauthorized("Student id not found in token");

        var query = new GetMaterialsBySessionQuery(sessionId, studentId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("download/{materialId}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Download(int materialId)
    {
        var studentId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(studentId))
            return Unauthorized();

        var material = await _unit.Materials.GetByIdAsync(materialId);
        if (material == null)
            return NotFound("Material not found");

        var isBooked = await _unit.Bookings.IsAlreadyBookedAsync(studentId, material.SessionId);
        if (!isBooked)
            return Forbid("You are not booked for this session");

        if (material.Type == "File" && !string.IsNullOrEmpty(material.FileUrl))
        {
            var stream = _fileService.GetFileStream(material.FileUrl);
            var fileName = Path.GetFileName(material.FileUrl);
            return File(stream, "application/octet-stream", fileName);
        }
        else if (material.Type == "Video" && !string.IsNullOrEmpty(material.VideoUrl))
        {
            return Ok(new { VideoUrl = material.VideoUrl });
        }
        else
        {
            return BadRequest("No downloadable content available");
        }
    }
}