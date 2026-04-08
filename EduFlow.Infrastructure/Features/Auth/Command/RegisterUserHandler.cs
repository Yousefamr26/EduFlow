using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Features.Auth.Command;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class RegisterUserHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(UserManager<ApplicationUser> userManager,
                              IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
       _unitOfWork  = unitOfWork;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return $"User with email {request.Email} already exists.";

        var newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.Phone,
            Name = request.Name,
            IsActive = true,
            IsAccessCodeVerified = false 
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded)
            return string.Join(", ", result.Errors.Select(e => e.Description));

        if (!string.IsNullOrEmpty(request.Role))
        {
            await _userManager.AddToRoleAsync(newUser, request.Role);
        }

        var accessCode = new EduFlow.Domain.Entities.AccessCodes
        {
            UserId = newUser.Id,
            CodeHash = Guid.NewGuid().ToString("N"),
            ExpiryDate = DateTime.UtcNow.AddHours(1), 
            IsUsed = false,
            Attempts = 0
        };

        await _unitOfWork.AccessCodes.AddAsync(accessCode);
        await _unitOfWork.SaveChangesAsync();

        return $"User created successfully. Access code: {accessCode.CodeHash}";
    }
}