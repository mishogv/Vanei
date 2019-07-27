namespace MIS.WebApp.Hubs
{
    using System.Threading.Tasks;

    using Ganss.XSS;

    using Microsoft.AspNetCore.SignalR;

    using ViewModels.View.Chat;

    public class ChatHub : Hub
    {
        private readonly IHtmlSanitizer sanitizer;

        public ChatHub(IHtmlSanitizer sanitizer)
        {
            this.sanitizer = sanitizer;
        }

        public async Task Send(string message)
        {
            var sanitizedMessage = this.sanitizer.Sanitize(message);

            await this.Clients.All.SendAsync(
                "NewMessage",
                new ChatHubMessageViewModel { Username = this.Context.User.Identity.Name, Text = sanitizedMessage, });
        }
    }
}