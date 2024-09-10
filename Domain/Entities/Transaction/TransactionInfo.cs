using Domain.Entities.Budget;

namespace Domain.Entities.Transaction;

public record TransactionInfo(double Amount, double BaseUserCurrencyAmount, bool IsActive, string Currency, BudgetInfo Budget);