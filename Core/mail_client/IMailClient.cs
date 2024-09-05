
namespace Core.mail_client;

public interface IMailClient
{
    Task SendMailAsync(MailData data);
}