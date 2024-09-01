using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;


namespace Application.MailClient;

public class SmtpMailClient : IMailClient
{
    private readonly IOptions<MailOptions> _options;

    public SmtpMailClient(IOptions<MailOptions> options) => _options = options;

    public async Task SendMailAsync(MailData data)
    {
        data.From ??= _options.Value.Email;
        data.DisplayName ??= _options.Value.DisplayName;

        using (var smtpClient = new SmtpClient())
        {
            try
            {
                Log.Information($"Start sending email for user {data.UserDisplayName}");
                await smtpClient.ConnectAsync(_options.Value.Host, _options.Value.Port, SecureSocketOptions.SslOnConnect);
                await smtpClient.AuthenticateAsync(_options.Value.Email, _options.Value.Password);
                await smtpClient.SendAsync(ConfigureMessage(data));
            } 
            catch (Exception ex)
            {
                Log.Error($"Error occured trying to send Email {ex.Message} for user {data.UserDisplayName}");
            }
            finally
            {
               await smtpClient.DisconnectAsync(true);
            }
        }
    }

    private MimeMessage ConfigureMessage(MailData data)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(data.DisplayName, data.From));
        mailMessage.To.Add(new MailboxAddress(data.UserDisplayName, data.To));
        mailMessage.Subject = data.Subject;

        var body = new BodyBuilder
        {
            HtmlBody = data.Body
        };
        mailMessage.Body = body.ToMessageBody();

        return mailMessage;
    }
}
