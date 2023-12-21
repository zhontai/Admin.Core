using DotNetCore.CAP;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Services.Msg.Events;

namespace ZhonTai.Admin.Services.Msg;

public class EmailService: ICapSubscribe
{
    private readonly Lazy<IOptions<EmailConfig>> _emailConfig;

    public EmailService(Lazy<IOptions<EmailConfig>> emailConfig)
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
        var emailConfig = _emailConfig.Value.Value;

        var builder = new BodyBuilder()
        {
            HtmlBody = @event.Body
        };
        var message = new MimeMessage()
        {
            Subject = @event.Subject,
            Body = builder.ToMessageBody()
        };
        message.From.Add(new MailboxAddress(emailConfig.FromEmail.Name, emailConfig.FromEmail.Address));
        message.To.Add(new MailboxAddress(@event.ToEmail.Name, @event.ToEmail.Address));

        using var client = new SmtpClient();
        await client.ConnectAsync(emailConfig.Host, emailConfig.Port, emailConfig.UseSsl);
        await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
