﻿using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Serilog;

namespace Application.kafka.consumers;

public abstract class ConsumerBackgroundService : BackgroundService
{
    private ConsumerConfig _config;
    protected IServiceScopeFactory _scopeFactory;
    protected JsonSerializerOptions _options;

    protected ConsumerBackgroundService(
        IOptions<KafkaOptions> options,
        IServiceScopeFactory scopeFactory
    )
    {
        _scopeFactory = scopeFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
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
            Log.Information($"Start consumer background service");
            _ = ConsumeAsync(GetTopic(), stoppingToken);
        }, stoppingToken);
    }

    private async Task ConsumeAsync(string topic, CancellationToken stoppingToken = default)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();

        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            Log.Information($"Start cycle after sub");
            var consumeResult = consumer.Consume(stoppingToken);
            Log.Information($"Start handling message {consumeResult.Message.Value} in topic {GetTopic()}");
            await HandleMessage(consumeResult.Message.Value);
        }

        consumer.Close();
    }

    protected abstract string GetTopic();

    protected abstract Task HandleMessage(string messageValue);  
}
