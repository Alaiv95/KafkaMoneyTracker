﻿using Domain.Enums;

namespace Application.handlers.budget.queries.CheckSpentBudget;

public class BudgetExceededMessage
{
    public string Category { get; set; }

    public double SpentAmount { get; set; }

    public string BudgetPeriod { get; set; }

    public double BudgetLimit { get; set; }

    public Guid UserId { get; set; }
}
