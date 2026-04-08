using EduFlow.Domain.Entities;
using System.Threading.Tasks;

namespace EduFlow.Application.Interfaces.Repositories
{
    public interface IAuthRepository:IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

       
        Task<AccessCodes> GenerateAccessCodeAsync(string userId);
        Task<AccessCodes> GetAccessCodeAsync(string code);
        Task<bool> MarkCodeAsUsedAsync(string codeId);

       
        Task<bool> IsEmailTakenAsync(string email);
    }
}