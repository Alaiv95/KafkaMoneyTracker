﻿using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Serilog;

namespace Application.kafka.producer;

public class EventsProducer : IEventsProducer
{
    readonly IProducer<string, string> _producer;

    public EventsProducer(IOptions<KafkaOptions> options)
    {
        var config = new ConsumerConfig
        {
            GroupId = options.Value.GroupId,
            BootstrapServers = options.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    public async Task Produce(string topic, Message<string, string> message)
    {
        Log.Information($"Producing message {message.Value} for topic {topic}");
        await _producer.ProduceAsync(topic, message);
    }
}
