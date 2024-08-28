using Application.kafka.consumers;
using Application.mediator.interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.kafka.consumer;

public class BudgetExceededConsumer : ConsumerBackgroundService
{
    public BudgetExceededConsumer(IOptions<KafkaOptions> options, IServiceScopeFactory scopeFactory) 
        : base(options, scopeFactory) { }

    protected override string GetTopic() => TopicConstants.BudgetExceededTopic;

    protected override async Task HandleMessage(string messageValue)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var transaction = JsonSerializer.Deserialize<List<Transaction>>(messageValue, _options);

            // Todo add mail sent to customer
        }
        catch (Exception ex)
        {
            //TODO add serilog
            Console.WriteLine(ex.Message);
        }
    }
}
