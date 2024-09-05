using System.Globalization;
using System.Text.Json;
using Application.exceptions;
using Application.handlers.budget.queries.CheckSpentBudget;
using Core.fileUtils;
using Core.mail_client;
using Infrastructure.Repositories.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace Application.kafka.consumers;

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
            var fileReader = scope.ServiceProvider.GetRequiredService<IFileReader>();

            var exceededDto = JsonSerializer.Deserialize<BudgetExceededDto>(messageValue, _options);

            var email = await GetTransactionUserEmail(exceededDto, authRepository);
            var message = await CreateMailDataAsync(exceededDto!, email, fileReader);

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


    private async Task<MailData> CreateMailDataAsync(BudgetExceededDto dto, string email, IFileReader fileReader)
    {
        var template = await CreateTemplate(dto, email, fileReader);
        
        return new MailData
        {
            UserDisplayName = email,
            To = email,
            Subject = "Превышение лимита бюджета",
            Body = template
        };
    }

    private async Task<string> CreateTemplate(BudgetExceededDto dto, string email, IFileReader fileReader)
    {
        var result = await fileReader.GetFileDataAsync("templates/template.html");

        return result
            .Replace("{email}", email)
            .Replace("{category}", dto.Category)
            .Replace("{period}", dto.BudgetPeriod)
            .Replace("{limit}", dto.BudgetLimit.ToString(CultureInfo.InvariantCulture))
            .Replace("{spent}", dto.SpentAmount.ToString(CultureInfo.InvariantCulture));
    }
}
