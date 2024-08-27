using Application.mediator.interfaces;

namespace Application.mediatorHandlers.auth;

public class RegisterUserCommand : IRequest
{
    public string Email { get; set; }

    public string Password { get; set; }
}
