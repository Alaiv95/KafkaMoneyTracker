namespace Core.common;

public record CreateTransactionMessage(
    Guid UserId,
    double Amount,
    string Currency,
    Guid BudgetId,
    double BaseUserCurrencyAmount
);