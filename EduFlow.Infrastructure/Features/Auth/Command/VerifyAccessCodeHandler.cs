using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Infrastructure.Features.Auth.Command;
using MediatR;

public class VerifyAccessCodeHandler : IRequestHandler<VerifyAccessCodeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public VerifyAccessCodeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(VerifyAccessCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Auths.GetUserByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("User not found");

        var code = await _unitOfWork.AccessCodes.GetByUserIdAsync(user.Id);
        if (code == null)
            throw new Exception("Access code not found");

        if (code.IsUsed || code.ExpiryDate < DateTime.UtcNow)
            throw new Exception("Code expired or already used");

        if (code.Attempts >= 5)
            throw new Exception("Maximum attempts reached");

        var hashedInputCode = Hash(request.Code); 

        if (code.CodeHash != hashedInputCode)
        {
            code.Attempts++;
            _unitOfWork.AccessCodes.Update(code);
            await _unitOfWork.SaveChangesAsync();

            throw new Exception("Incorrect code");
        }

        code.IsUsed = true;
        code.Attempts = 0;

        user.IsAccessCodeVerified = true;

        _unitOfWork.AccessCodes.Update(code);
        _unitOfWork.Auths.Update(user);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private string Hash(string input)
    {
       
        return input; 
    }
}