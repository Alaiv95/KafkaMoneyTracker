﻿namespace Application.handlers.budget.queries.GetBudgetList;

public class BudgetLookUpVm
{
    public double BudgetLimit { get; set; }

    public int DurationInDays { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CategoryId { get; set; }
}
