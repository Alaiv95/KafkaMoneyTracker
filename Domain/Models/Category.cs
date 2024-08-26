using Domain.Enums;

namespace Domain.Models;

public class Category
{
    public Guid Id { get; set; }

    public CategoryType CategoryType { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public ICollection<Budget> Budgets { get; set; }
}