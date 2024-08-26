using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.budget.queries;

public class BudgetLookUpDto
{
    public Double BudgetLimit { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public Guid CategoryId { get; set; }
}
