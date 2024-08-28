using Application.mediator.interfaces;

namespace Application.handlers.transactions;

public class CreateTransactionCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public double Amount { get; set; }

    public Guid CategoryId { get; set; }
}
