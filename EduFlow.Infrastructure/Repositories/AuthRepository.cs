using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AuthRepository : GenericRepository<ApplicationUser>, IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EduDbContext _context;

    public AuthRepository(UserManager<ApplicationUser> userManager, EduDbContext context)
        : base(context)
    {
        _userManager = userManager;
        _context = context;
    }

    // 👑 Users
    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        => await _userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        => await _userManager.FindByIdAsync(userId);

    public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => await _userManager.CheckPasswordAsync(user, password);

    public async Task<bool> IsEmailTakenAsync(string email)
        => await _userManager.FindByEmailAsync(email) != null;

    // ✅ مهم جدًا (Update لليوزر)
    public async Task UpdateUserAsync(ApplicationUser user)
    {
        await _userManager.UpdateAsync(user);
    }

    // 🔑 Access Codes

    public async Task<AccessCodes> GenerateAccessCodeAsync(string userId)
    {
        var rawCode = Guid.NewGuid().ToString("N");

        var code = new AccessCodes
        {
            UserId = userId,
            CodeHash = Hash(rawCode), // ✅ نخزن hash
            ExpiryDate = DateTime.UtcNow.AddHours(24),
            IsUsed = false,
            Attempts = 0
        };

        await _context.AccessCodes.AddAsync(code);

        // ❗ متعملش Save هنا (UnitOfWork هو اللي يعمل)

        // رجّع الكود الحقيقي عشان يتبعت لليوزر
        code.CodeHash = rawCode;

        return code;
    }

    // ✅ هات الكود باليوزر (الأهم)
    public async Task<AccessCodes> GetAccessCodeByUserIdAsync(string userId)
    {
        return await _context.AccessCodes
            .Where(x => x.UserId == userId && !x.IsUsed)
            .OrderByDescending(x => x.ExpiryDate)
            .FirstOrDefaultAsync();
    }

    // ❌ دي خليها تتشال أو تتستخدم بحذر
    public async Task<AccessCodes> GetAccessCodeAsync(string code)
    {
        return await _context.AccessCodes
            .FirstOrDefaultAsync(c => c.CodeHash == code);
    }

    public void UpdateAccessCode(AccessCodes code)
    {
        _context.AccessCodes.Update(code);
    }

    // 🔐 Hash helper
    private string Hash(string input)
    {
        // حط SHA256 هنا
        return input; // ⚠️ غيرها في production
    }

    public async Task<bool> MarkCodeAsUsedAsync(string codeId)
    {
        var code = await _context.AccessCodes.FindAsync(codeId);

        if (code == null)
            return false;

        code.IsUsed = true;

        _context.AccessCodes.Update(code);

        // ❗ متعملش SaveChanges هنا لو شغال بـ UnitOfWork
        // سيبها للـ UnitOfWork

        return true;
    }
}