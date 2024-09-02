using Application.Dtos;
using Application.handlers.budget.queries.GetBudgetList;
using Application.kafka;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Application.specs;
using Confluent.Kafka;
using Infrastructure.Repositories;
using System.Text.Json;
using Domain.Entities.Budget;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;

namespace Application.handlers.budget.queries.CheckSpentBudget;

public class CheckSpentBudgetQueryHandler : IRequestHandler<CheckSpentBudgetQuery, bool>
{
    private readonly IGenericRepository<Transaction, TransactionEntity> _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IEventsProducer _eventsProducer;
    private readonly BudgetSpecs _budgetSpec;
    private readonly TransactionSpecs _transactionSpec;

    public CheckSpentBudgetQueryHandler(
        IGenericRepository<Transaction, TransactionEntity> transactionRepository,
        IBudgetRepository budgetRepository,
        IEventsProducer eventsProducer,
        BudgetSpecs budgetSpecs,
        TransactionSpecs transactionSpecs
    )
    {
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
        _eventsProducer = eventsProducer;
        _budgetSpec = budgetSpecs;
        _transactionSpec = transactionSpecs;
    }

    public async Task<bool> Handle(CheckSpentBudgetQuery query)
    {
        var budget = await GetActiveBudget(query);

        if (budget is null)
        {
            return false;
        }

        var transactions = await GetTransactionsFromBudgetPeriod(budget, query);
        var spentAmount = GetSpentAmount(transactions);
        var isExceeded = IsBudgetForCategoryExceeded(budget, spentAmount);

        if (isExceeded)
        {
            await ProduceMessage(query, budget, spentAmount);
        }

        return isExceeded;
    }

    private async Task ProduceMessage(CheckSpentBudgetQuery query, BudgetEntity budget, double spentAmount)
    {
        var messageData = new BudgetExceededDto
        {
            BudgetLimit = budget.BudgetLimit.Amount,
            BudgetPeriod = $"{budget.CreatedAt} - {budget.CreatedAt.AddDays(budget.BudgetLimit.Duration)}",
            Category = budget.CategoryId.ToString(),
            SpentAmount = spentAmount,
            UserId = query.UserId
        };

        await _eventsProducer.Produce(TopicConstants.BudgetExceededTopic, new Message<string, string>
        {
            Key = $"{query.UserId.ToString()}{query.CategoryId.ToString()}",
            Value = JsonSerializer.Serialize(messageData)
        });
    }

    private bool IsBudgetForCategoryExceeded(BudgetEntity budget, double moneySpent)
    {
        var transactionLimit = budget.BudgetLimit.Amount;

        return Math.Abs(moneySpent) > transactionLimit;
    }

    private double GetSpentAmount(List<TransactionEntity> transactions)
    {
        return transactions.Where(t => t.Money.Amount < 0).Sum(t => t.Money.Amount);
    }

    private async Task<BudgetEntity?> GetActiveBudget(CheckSpentBudgetQuery command)
    {
        var filter = new GetBudgetListQuery
        {
            CategoryId = command.CategoryId,
            UserId = command.UserId,
            DateFrom = DateTime.UtcNow,
        };

        var budgetList = await _budgetRepository.SearchAsync(_budgetSpec.Build(filter));

        return budgetList.FirstOrDefault();
    }

    private async Task<List<TransactionEntity>> GetTransactionsFromBudgetPeriod(BudgetEntity budget, CheckSpentBudgetQuery command)
    {
        var dateFrom = budget.CreatedAt.ToUniversalTime();
        var dateTo = dateFrom.AddDays(budget.BudgetLimit.Duration);

        var filter = new BaseSearchDto
        {
            CategoryId = command.CategoryId,
            UserId = command.UserId,
            DateFrom = dateFrom,
            DateTo = dateTo,
        };

        var transactions = await _transactionRepository.SearchAsync(_transactionSpec.Build(filter));

        return transactions.Where(t => t.IsActive).ToList();
    }
}
