using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Auth.Command
{
    public record CreateUserCommand(string Name, string Email, string Phone, string Password, string Role) : IRequest<string>;

}
