namespace MIS.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSender : IEmailSender
    {
        //TODO : Refactor make configuration file
        private const string SenderEmail = "no-reply@mis.com";
        private const string NameOfTheSender = "MIS";
        private const string NameForApi = "mis";
        private const string ApiKey = "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";

        public EmailSender(IConfiguration configuration)
        {
            this.SendGridKey = ApiKey;
        }

        public string SendGridUser { get; set; }

        public string SendGridKey { get; set; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return this.Execute(this.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(SenderEmail, NameOfTheSender),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}