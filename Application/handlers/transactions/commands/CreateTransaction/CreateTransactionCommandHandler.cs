using System.Text.Json;
using Application.exceptions;
using Application.kafka;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Application.utils.CurrencyConverter;
using Confluent.Kafka;
using Core.common;
using Domain.Entities.Transaction;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;

namespace Application.handlers.transactions.commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, bool>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IEventsProducer _eventsProducer;
    private readonly IAuthRepository _authRepository;
    private readonly IConverter<Money> _converter;

    public CreateTransactionCommandHandler(
        IBudgetRepository budgetRepository,
        IEventsProducer eventsProducer,
        IAuthRepository authRepository,
        IConverter<Money> converter)
    {
        _budgetRepository = budgetRepository;
        _eventsProducer = eventsProducer;
        _authRepository = authRepository;
        _converter = converter;
    }

    public async Task<bool> Handle(CreateTransactionCommand command)
    {
        var budget = await _budgetRepository.GetByIdAsync(command.BudgetId);

        if (budget is null)
        {
            throw new NotFoundException($"Active budget with id {command.BudgetId} not found.");
        }

        var user = await _authRepository.GetByIdAsync(command.UserId);
        var baseCurrencyAmount = await _converter.ConvertOne(
            command.Currency,
            command.Amount,
            user?.MainCurrency ?? "RUB"
        );

        var message = new CreateTransactionMessage(
            UserId: command.UserId,
            BudgetId: command.BudgetId,
            Currency: command.Currency,
            Amount: command.Amount,
            BaseUserCurrencyAmount: baseCurrencyAmount
        );

        await _eventsProducer.Produce(TopicConstants.TransactionTopc, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(message)
        });

        return true;
    }
}