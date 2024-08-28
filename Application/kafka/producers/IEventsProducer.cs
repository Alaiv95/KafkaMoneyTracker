using Confluent.Kafka;

namespace Application.kafka.producer;

public interface IEventsProducer
{
    Task Produce(string topic, Message<string, string> message);
}
