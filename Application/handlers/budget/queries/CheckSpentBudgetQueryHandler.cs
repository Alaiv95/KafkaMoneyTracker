using Application.Dtos;
using Application.kafka;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Application.mediatorHandlers.budget.queries;
using Application.specs;
using Confluent.Kafka;
using Domain.Models;
using Infrastructure.Repositories;
using System.Text.Json;

namespace Application.handlers.budget.queries;

public class CheckSpentBudgetQueryHandler : IRequestHandler<CheckSpentBudgetQuery, bool>
{
    private readonly IGenericRepository<Transaction> _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IEventsProducer _eventsProducer;
    private readonly BudgetSpecs _budgetSpec;
    private readonly TransactionSpecs _transactionSpec;

    public CheckSpentBudgetQueryHandler(
        IGenericRepository<Transaction> transactionRepository,
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

    public async Task<bool> Handle(CheckSpentBudgetQuery command)
    {
        var budget = await GetActiveBudget(command);

        if (budget is null)
        {
            return false;
        }

        var transactions = await GetTransactionsFromBudgetPeriod(budget, command);
        var isExceeded = IsBudgetForCategoryExceeded(budget, transactions);

        if (isExceeded)
        {
            await _eventsProducer.Produce(TopicConstants.BudgetExceededTopic, new Message<string, string>
            {
                Key = $"{command.UserId.ToString()}{command.CategoryId.ToString()}",
                Value = JsonSerializer.Serialize(transactions)
            });
        }

        return isExceeded;
    }

    private bool IsBudgetForCategoryExceeded(Budget budget, List<Transaction> transactions)
    {
        var transactionLimit = budget.BudgetLimit;
        var moneySpent = transactions.Where(t => t.Amount < 0).Sum(t=> t.Amount);
        
        return Math.Abs(moneySpent) > transactionLimit;
    }

    private async Task<Budget?> GetActiveBudget(CheckSpentBudgetQuery command)
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

    private async Task<List<Transaction>> GetTransactionsFromBudgetPeriod(Budget budget, CheckSpentBudgetQuery command)
    {
        var dateFrom = budget.CreatedAt.ToUniversalTime();
        var dateTo = dateFrom.AddDays(budget.DurationInDays);

        var filter = new BaseSearchDto
        {
            CategoryId = command.CategoryId,
            UserId = command.UserId,
            DateFrom = dateFrom,
            DateTo = dateTo,
        };

        return await _transactionRepository.SearchAsync(_transactionSpec.Build(filter));
    }
}
