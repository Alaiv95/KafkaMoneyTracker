using Application.mediator.interfaces;

namespace Application.mediatorHandlers.auth;

public class RegisterUserCommand : IRequest<bool>
{
    public string Email { get; set; }

    public string Password { get; set; }
}
