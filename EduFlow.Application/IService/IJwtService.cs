using EduFlow.Domain.Entities;

public interface IJwtService
{
    Task<string> GenerateToken(ApplicationUser user);
}