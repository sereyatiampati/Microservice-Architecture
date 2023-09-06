
using MailKit.Net.Smtp;
using MimeKit;
using TheJitu_commerce_EmailService.Models;

namespace TheJitu_commerce_EmailService.Services
{
    public class EmailSendService
    {

        public async Task SendEmail(UserMessage res, string message)
        {
            MimeMessage message1 = new MimeMessage();
            message1.From.Add(new MailboxAddress("THE Jitu E-Commerce ", "joepay592@gmail.com"));

            // Set the recipient's email address
            message1.To.Add(new MailboxAddress(res.Name, res.Email));

            message1.Subject = "Welcome to TheJitu Shopping Site";

            var body = new TextPart("html")
            {
                Text = message.ToString()
            };
            message1.Body = body;

            var client = new SmtpClient();

            client.Connect("smtp.gmail.com", 587, false);

            client.Authenticate("joepay592@gmail.com", "oczc ivvn vihe ldaa");

            await client.SendAsync(message1);

            await client.DisconnectAsync(true);
        }
    }
}
