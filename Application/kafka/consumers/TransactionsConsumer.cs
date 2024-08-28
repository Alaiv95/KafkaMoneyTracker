using Application.handlers.budget.queries;
using Application.kafka.consumers;
using Application.mediator.interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

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
            var transactionRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Transaction>>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var transaction = JsonSerializer.Deserialize<Transaction>(messageValue, _options);

            if (transaction != null)
            {
                await transactionRepository.AddAsync(transaction);

                var checkBudgetCommand = new CheckSpentBudgetQuery
                {
                    UserId = transaction.UserId,
                    CategoryId = transaction.CategoryId,
                };

                await mediator.HandleRequest(checkBudgetCommand);
            }
        }
        catch (Exception ex)
        {
            //TODO add serilog
            Console.WriteLine(ex.Message);
        }
    }
}
