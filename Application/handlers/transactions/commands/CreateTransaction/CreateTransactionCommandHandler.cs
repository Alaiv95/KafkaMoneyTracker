using Application.exceptions;
using Application.kafka;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Confluent.Kafka;
using Domain.Models;
using Infrastructure.Repositories;
using System.Text.Json;

namespace Application.handlers.transactions.commands.CreateTransaction;
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, bool>
{
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IEventsProducer _eventsProducer;

    public CreateTransactionCommandHandler(
        IGenericRepository<Category> categoryRepository,
        IEventsProducer eventsProducer
    )
    {
        _categoryRepository = categoryRepository;
        _eventsProducer = eventsProducer;
    }

    public async Task<bool> Handle(CreateTransactionCommand command)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId);

        if (category is null)
        {
            throw new NotFoundException($"category {command.CategoryId} not found");
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = command.Amount,
            CategoryId = command.CategoryId,
            UserId = command.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _eventsProducer.Produce(TopicConstants.TransactionTopc, new Message<string, string>
        {
            Key = transaction.Id.ToString(),
            Value = JsonSerializer.Serialize(transaction)
        });

        return true;
    }
}
