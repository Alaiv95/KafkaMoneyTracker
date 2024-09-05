using Application.exceptions;
using Application.kafka;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Confluent.Kafka;
using Infrastructure.Repositories;
using System.Text.Json;
using Application.handlers.budget.queries.GetBudgetList;
using Domain.Entities;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Microsoft.IdentityModel.Tokens;

namespace Application.handlers.transactions.commands.CreateTransaction;
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, bool>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IEventsProducer _eventsProducer;
    private readonly BudgetSpecs _budgetSpecs;

    public CreateTransactionCommandHandler(
        IBudgetRepository budgetRepository,
        IEventsProducer eventsProducer,
        BudgetSpecs budgetSpecs
    )
    {
        _budgetRepository = budgetRepository;
        _eventsProducer = eventsProducer;
        _budgetSpecs = budgetSpecs;
    }

    public async Task<bool> Handle(CreateTransactionCommand command)
    {
        var budget = await _budgetRepository.GetByIdAsync(command.BudgetId);

        if (budget is null)
        {
            throw new NotFoundException($"Active budget with id {command.BudgetId} not found.");
        }

        await _eventsProducer.Produce(TopicConstants.TransactionTopc, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        return true;
    }
}
