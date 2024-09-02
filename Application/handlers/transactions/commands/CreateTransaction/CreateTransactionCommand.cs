using Application.mediator.interfaces;

namespace Application.handlers.transactions.commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public double Amount { get; set; }

    public Guid BudgetId { get; set; }
    
    public string Currency { get; set; }
}
