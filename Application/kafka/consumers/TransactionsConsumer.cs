using Confluent.Kafka;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.kafka.consumer;

public class TransactionsConsumer : BackgroundService
{
    private ConsumerConfig _config;
    private IServiceScopeFactory _scopeFactory;
    private JsonSerializerOptions _options;

    public TransactionsConsumer(
        IOptions<KafkaOptions> options,
        IServiceScopeFactory scopeFactory
    )
    {
        _scopeFactory = scopeFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _config = new ConsumerConfig
        {
            GroupId = options.Value.GroupId,
            BootstrapServers = options.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _ = ConsumeAsync(TopicConstants.TransactionTopc, stoppingToken);
        }, stoppingToken);
    }

    private async Task ConsumeAsync(string topic, CancellationToken stoppingToken = default)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();

        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested) 
        {
            var consumeResult = consumer.Consume(stoppingToken);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var transactionRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Transaction>>();
                var budgetRepository = scope.ServiceProvider.GetRequiredService<IBudgetRepository>();

                var transaction = JsonSerializer.Deserialize<Transaction>(consumeResult.Message.Value, _options);

                if (transaction != null)
                {
                    await transactionRepository.AddAsync(transaction);
                }
            } 
            catch (Exception ex)
            {
                //TODO add serilog
                Console.WriteLine(ex.Message);
            }
        }

        consumer.Close();
    }
}
