using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test")]
public class TestAuthController : ControllerBase
{
    [HttpGet("teacher-only")]
    [Authorize(Roles = "Teacher")]
    public IActionResult TeacherOnly()
    {
        var userId = User.FindFirst("id")?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(r => r.Value).ToList();

        return Ok(new
        {
            Message = "Authorization OK",
            UserId = userId,
            Email = email,
            Roles = roles
        });
    }
}