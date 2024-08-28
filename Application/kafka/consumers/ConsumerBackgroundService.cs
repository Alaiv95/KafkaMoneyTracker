using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.kafka.consumers;

public abstract class ConsumerBackgroundService : BackgroundService
{
    protected ConsumerConfig _config;
    protected IServiceScopeFactory _scopeFactory;
    protected JsonSerializerOptions _options;

    public ConsumerBackgroundService(
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
            _ = ConsumeAsync(GetTopic(), stoppingToken);
        }, stoppingToken);
    }

    protected async Task ConsumeAsync(string topic, CancellationToken stoppingToken = default)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();

        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);
            await HandleMessage(consumeResult.Message.Value);
        }

        consumer.Close();
    }

    protected abstract string GetTopic();

    protected abstract Task HandleMessage(string messageValue);  
}
