
namespace Application.MailClient;

public interface IMailClient
{
    Task SendMailAsync(MailData data);
}