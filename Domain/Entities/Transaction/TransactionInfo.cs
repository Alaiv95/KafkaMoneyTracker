using Domain.Entities.Budget;

namespace Domain.Entities.Transaction;

public record TransactionInfo(double Amount, bool IsActive, string Currency, BudgetInfo Budget);