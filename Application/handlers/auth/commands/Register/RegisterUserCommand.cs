using Application.mediator.interfaces;

namespace Application.handlers.auth.commands.Register;

public class RegisterUserCommand : IRequest<bool>
{
    public string Email { get; set; }

    public string Password { get; set; }
    
    public string MainCurrency { get; set; }
}
