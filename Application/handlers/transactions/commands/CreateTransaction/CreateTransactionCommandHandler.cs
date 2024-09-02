using Application.exceptions;
using Application.kafka;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Confluent.Kafka;
using Infrastructure.Repositories;
using System.Text.Json;
using Domain.Entities;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;

namespace Application.handlers.transactions.commands.CreateTransaction;
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, bool>
{
    private readonly IGenericRepository<Category, CategoryEntity> _categoryRepository;
    private readonly IEventsProducer _eventsProducer;

    public CreateTransactionCommandHandler(
        IGenericRepository<Category, CategoryEntity> categoryRepository,
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

        await _eventsProducer.Produce(TopicConstants.TransactionTopc, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        return true;
    }
}
