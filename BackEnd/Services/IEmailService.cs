using System.Net;
using System.Net.Mail;
namespace Referly.Services;
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var client = new SmtpClient
        {
            Host = _config["EmailSettings:Host"],
            Port = int.Parse(_config["EmailSettings:Port"]),
            Credentials = new NetworkCredential(
                _config["EmailSettings:Username"],
                _config["EmailSettings:Password"]),
            EnableSsl = true
        };

        var message = new MailMessage("no-reply@yourdomain.com", to, subject, body) { IsBodyHtml = true };
        await client.SendMailAsync(message);
    }
}
