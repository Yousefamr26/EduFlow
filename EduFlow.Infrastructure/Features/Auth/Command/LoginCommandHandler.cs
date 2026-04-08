using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Infrastructure.Features.Auth.Command;
using MediatR;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IUnitOfWork _unit;
    private readonly IJwtService _jwt;

    public LoginCommandHandler(IUnitOfWork unit, IJwtService jwt)
    {
        _unit = unit;
        _jwt = jwt;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unit.Auths.GetUserByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("Invalid Email or Password");

        var valid = await _unit.Auths.CheckPasswordAsync(user, request.Password);
        if (!valid)
            throw new Exception("Invalid Email or Password");

        if (!user.IsAccessCodeVerified)
            throw new Exception("Access Code Required");

        return await _jwt.GenerateToken(user);
    }
}