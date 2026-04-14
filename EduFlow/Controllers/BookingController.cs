using EduFlow.Infrastructure.Features.BookingSystem.Command;
using EduFlow.Infrastructure.Features.BookingSystem.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/bookings")]
[Authorize(Roles = "Student")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string GetStudentIdFromToken()
    {
        var studentId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(studentId))
            throw new UnauthorizedAccessException("Student id not found in token.");
        return studentId;
    }

    [HttpPost]
    public async Task<IActionResult> Book([FromBody] BookSessionDto dto)
    {
        try
        {
            var studentId = GetStudentIdFromToken();
            var command = new BookSessionCommand(studentId, dto.SessionId);
            var result = await _mediator.Send(command);

            if (result < 0)
            {
                // Negative ID indicates waiting list entry
                return Ok(new { 
                    WaitingListEntryId = -result, 
                    Status = "Waiting",
                    Message = "Session is full. You have been added to the waiting list." 
                });
            }

            return Ok(new { 
                BookingId = result, 
                Status = "Booked",
                Message = "Successfully booked the session."
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var studentId = GetStudentIdFromToken();
            var command = new CancelBookingCommand(id, studentId);
            await _mediator.Send(command);
            return Ok();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("my-bookings")]
    public async Task<IActionResult> MyBookings()
    {
        try
        {
            var studentId = GetStudentIdFromToken();
            var query = new GetStudentBookingsQuery(studentId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}

public record BookSessionDto(int SessionId);