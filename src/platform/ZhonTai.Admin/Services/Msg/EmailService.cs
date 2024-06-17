using DotNetCore.CAP;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Services.Msg.Events;

namespace ZhonTai.Admin.Services.Msg;

public class EmailService: ICapSubscribe
{
    private readonly IOptions<EmailConfig> _emailConfig;

    public EmailService(IOptions<EmailConfig> emailConfig)
    {
        _emailConfig = emailConfig;
    }

    /// <summary>
    /// 邮件单发
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    [NonAction]
    [CapSubscribe("zhontai.admin.emailSingleSend")]
    public async Task SingleSendAsync(EamilSingleSendEvent @event)
    {
        var emailConfig = _emailConfig.Value;

        var builder = new BodyBuilder()
        {
            HtmlBody = @event.Body
        };
        var message = new MimeMessage()
        {
            Subject = @event.Subject,
            Body = builder.ToMessageBody()
        };

        var fromEmailName = @event.FromEmail!=null && @event.FromEmail.Name.NotNull() ? @event.FromEmail.Name : emailConfig.FromEmail.Name;
        var fromEmailAddress = @event.FromEmail != null && @event.FromEmail.Address.NotNull() ? @event.FromEmail.Address : emailConfig.FromEmail.Address;
        message.From.Add(new MailboxAddress(fromEmailName, fromEmailAddress));
        message.To.Add(new MailboxAddress(@event.ToEmail.Name, @event.ToEmail.Address));

        using var client = new SmtpClient();
        await client.ConnectAsync(emailConfig.Host, emailConfig.Port, emailConfig.UseSsl);
        await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
