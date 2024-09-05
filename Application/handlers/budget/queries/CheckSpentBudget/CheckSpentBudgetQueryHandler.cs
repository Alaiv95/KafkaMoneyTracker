using Application.Dtos;
using Application.handlers.budget.queries.GetBudgetList;
using Application.kafka;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Confluent.Kafka;
using Infrastructure.Repositories;
using System.Text.Json;
using Domain.Entities.Budget;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Core.common;

namespace Application.handlers.budget.queries.CheckSpentBudget;

public class CheckSpentBudgetQueryHandler : IRequestHandler<CheckSpentBudgetQuery, bool>
{
    private readonly IGenericRepository<Transaction, TransactionEntity> _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IEventsProducer _eventsProducer;
    private readonly TransactionSpecs _transactionSpec;

    public CheckSpentBudgetQueryHandler(
        IGenericRepository<Transaction, TransactionEntity> transactionRepository,
        IBudgetRepository budgetRepository,
        IEventsProducer eventsProducer,
        TransactionSpecs transactionSpecs
    )
    {
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
        _eventsProducer = eventsProducer;
        _transactionSpec = transactionSpecs;
    }

    public async Task<bool> Handle(CheckSpentBudgetQuery query)
    {
        var budget = await _budgetRepository.GetByIdAsync(query.BudgetId);

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
            Key = $"{query.UserId.ToString()}{query.BudgetId.ToString()}",
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
    
    //todo refactor to find transactions by budgetId
    private async Task<List<TransactionEntity>> GetTransactionsFromBudgetPeriod(BudgetEntity budget, CheckSpentBudgetQuery command)
    {
        var dateFrom = budget.CreatedAt.ToUniversalTime();
        var dateTo = dateFrom.AddDays(budget.BudgetLimit.Duration);

        var filter = new BaseBudgetSearchFilter
        {
            BudgetId = command.BudgetId,
            UserId = command.UserId,
            DateFrom = dateFrom,
            DateTo = dateTo,
        };

        var transactions = await _transactionRepository.SearchAsync(_transactionSpec.Build(filter));

        return transactions.Where(t => t.IsActive).ToList();
    }
}
