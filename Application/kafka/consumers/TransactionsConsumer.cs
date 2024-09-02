using Application.handlers.budget.queries.CheckSpentBudget;
using Application.kafka.consumers;
using Application.mediator.interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Application.handlers.transactions.commands.CreateTransaction;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Serilog;

namespace Application.kafka.consumer;

public class TransactionsConsumer : ConsumerBackgroundService
{
    public TransactionsConsumer(IOptions<KafkaOptions> options, IServiceScopeFactory scopeFactory) 
        : base(options, scopeFactory) { }

    protected override string GetTopic() => TopicConstants.TransactionTopc;

    protected override async Task HandleMessage(string messageValue)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var transactionRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Transaction, TransactionEntity>>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var command = JsonSerializer.Deserialize<CreateTransactionCommand>(messageValue, _options);
            
            if (command != null)
            {
                var transaction = TransactionEntity.Create(command.UserId, Money.Create(command.Amount, "RUB"), command.CategoryId);
                
                await transactionRepository.AddAsync(transaction);

                var checkBudgetCommand = new CheckSpentBudgetQuery
                {
                    UserId = transaction.UserId,
                    CategoryId = transaction.CategoryId,
                };

                await mediator.HandleRequest(checkBudgetCommand);
            }
            
            Log.Information($"Successfully consumed message {messageValue} in topic {GetTopic()}");
        }
        catch (Exception ex)
        {
            Log.Error($"Error occured consuming message {messageValue} in topic {GetTopic()} with exception {ex}");
        }
    }
}
