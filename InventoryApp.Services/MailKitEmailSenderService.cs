using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using InventoryApp.Contracts.Options;
using InventoryApp.Contracts.Dtos;

namespace InventoryApp.Services
{
    public class MailKitEmailSenderService
    {
        private readonly IOptions<SmtpClientOptions> _smtpClientOptions;

        public MailKitEmailSenderService(IOptions<SmtpClientOptions> smtpClientOptions)
        {
            _smtpClientOptions = smtpClientOptions;
        }

        public async Task<string> SendEmail(EmailDto data)
        {
            var client = new SmtpClient();

            try
            {

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(data.FromName, data.FromAddress));
                message.To.Add(new MailboxAddress(data.ToName, data.ToAddress));
                message.Subject = data.Subject;

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = data.Text
                };
                message.Body = bodyBuilder.ToMessageBody();

                await client.ConnectAsync(_smtpClientOptions.Value.Address, _smtpClientOptions.Value.Port, false);
                await client.AuthenticateAsync(data.FromAddress, data.FromPassword);
                await client.SendAsync(message);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }

            return "Ok";
        }
    }

}
