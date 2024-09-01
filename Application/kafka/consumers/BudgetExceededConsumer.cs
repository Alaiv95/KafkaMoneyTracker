using Application.exceptions;
using Application.handlers.budget.queries.CheckSpentBudget;
using Application.kafka.consumers;
using Application.MailClient;
using Application.mediator.interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Serilog;

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
            var mailClient = scope.ServiceProvider.GetRequiredService<IMailClient>();
            var authRepository = scope.ServiceProvider.GetRequiredService<IAuthRepository>();

            var exceededDto = JsonSerializer.Deserialize<BudgetExceededDto>(messageValue, _options);

            var email = await GetTransactionUserEmail(exceededDto, authRepository);
            var message = CreateMailData(exceededDto!, email);

            await mailClient.SendMailAsync(message);
            
            Log.Information($"Successfully consumed message {messageValue} in topic {GetTopic()}");
        }
        catch (Exception ex)
        {
            Log.Error($"Error occured consuming message {messageValue} in topic {GetTopic()} with exception {ex.Message}");
        }
    }

    private async Task<string> GetTransactionUserEmail(BudgetExceededDto? dto, IAuthRepository authRepository)
    {
        if (dto is null || dto.UserId == Guid.Empty)
        {
            throw new ApplicationException($"Ошибка получения данных из kafka");
        }

        var user = await authRepository.GetByIdAsync(dto.UserId);

        if (user == null)
        {
            throw new NotFoundException($"Пользователь с id {dto.UserId} не найден");
        }

        return user.Email;
    }


    private MailData CreateMailData(BudgetExceededDto dto, string email)
    {
        var bodyMessage = $"Уважаемый {email}. Бюджет в категории {dto.Category} за период {dto.BudgetPeriod} превышен." +
            $" Указанный лимит = {dto.BudgetLimit}, расходы составляют {dto.SpentAmount}.";

        return new MailData
        {
            UserDisplayName = email,
            To = email,
            Subject = "Превышение лимита бюджета",
            Body = bodyMessage
        };
    }
}
