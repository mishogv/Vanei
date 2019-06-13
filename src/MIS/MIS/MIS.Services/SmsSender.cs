namespace MIS.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;

    using Options;

    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class SmsSender : ISmsSender
    {
        public AuthMessageSender(IOptions<SmsOptions> optionsAccessor)
        {
            this.Options = optionsAccessor.Value;
        }

        public SmsOptions Options { get; }  // set only via Secret Manager
        

        public Task SendSmsAsync(string number, string message)
        {
            ASPSMS.SMS SMSSender = new ASPSMS.SMS();

            SMSSender.Userkey = Options.SmsAccountIdentification;
            SMSSender.Password = Options.SmsAccountPassword;
            SMSSender.Originator = Options.SmsAccountFrom;

            SMSSender.AddRecipient(number);
            SMSSender.MessageData = message;

            SMSSender.SendTextSMS();

            return Task.FromResult(0);
        }
    }

    public interface ISmsSender
    {
        SmsOptions Options { get; }

        Task SendSmsAsync(string number, string message);
    }
}