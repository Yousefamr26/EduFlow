using EduFlow.Infrastructure.Features.WaitingList.Commands;
using EduFlow.Infrastructure.Features.WaitingList.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduFlow.Controllers
{
    [ApiController]
    [Route("api/waiting-list")]
    [Authorize]
    public class WaitingListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WaitingListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserIdFromToken()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User id not found in token.");
            return userId;
        }

        /// <summary>
        /// Join the waiting list for a session
        /// </summary>
        [HttpPost("join")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> JoinWaitingList([FromBody] JoinWaitingListDto dto)
        {
            try
            {
                var studentId = GetUserIdFromToken();
                var command = new AddToWaitingListCommand(studentId, dto.SessionId);
                var entryId = await _mediator.Send(command);
                return Ok(new { WaitingListEntryId = entryId, Message = "Added to waiting list" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get student's waiting list positions across all sessions
        /// </summary>
        [HttpGet("my-positions")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyWaitingListPositions()
        {
            try
            {
                var studentId = GetUserIdFromToken();
                var query = new GetStudentWaitingListPositionsQuery(studentId);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get waiting list for a specific session (Admin only)
        /// </summary>
        [HttpGet("session/{sessionId}")]
        [Authorize(Roles = "Admin,Teacher")]
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

    public record JoinWaitingListDto(int SessionId);
}
