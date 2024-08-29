using Application.mediator.interfaces;

namespace Application.handlers.auth.commands.Login;

public class LoginCommand : IRequest<string>
{
    public string Email { get; set; }

    public string Password { get; set; }
}
