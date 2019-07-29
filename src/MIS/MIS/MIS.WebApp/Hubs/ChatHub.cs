namespace MIS.WebApp.Hubs
{
    using System.Threading.Tasks;

    using Common;

    using Ganss.XSS;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.View.Chat;

    public class ChatHub : Hub
    {
        private const string MessageJoinGroupTemplate = "{0} has joined the group {1}";
        private const string MessageLeftGroupTemplate = "{0} has left the group {1}";

        private readonly IHtmlSanitizer sanitizer;
        private readonly IMessageService messageService;
        private readonly UserManager<MISUser> userManager;

        public ChatHub(IHtmlSanitizer sanitizer, 
            IMessageService messageService,
            UserManager<MISUser> userManager)
        {
            this.sanitizer = sanitizer;
            this.messageService = messageService;
            this.userManager = userManager;
        }

        public async Task AddToGroup(string companyId)
        {
            var username = this.Context.User.Identity.Name;
            var message = await this.messageService.CreateAsync(companyId, username, MessageJoinGroupTemplate, true);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, message.Company.Name);
            message.Username = GlobalConstants.System;

            await this.Clients.Group(message.Company.Name)
                         .SendAsync("NewMessage", message.MapTo<ChatHubMessageViewModel>());
        }

        public async Task RemoveFromGroup(string companyId)
        {
            var username = this.Context.User.Identity.Name;
            var message = await this.messageService.CreateAsync(companyId, username, MessageLeftGroupTemplate, true);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, message.Company.Name);

            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, message.Company.Name);
            message.Username = GlobalConstants.System;

            await this.Clients.Group(message.Company.Name)
                      .SendAsync("NewMessage", message.MapTo<ChatHubMessageViewModel>());
        }

        public async Task Send(string companyId, string message)
        {
            var username = this.Context.User.Identity.Name;
            var sanitizedMessage = this.sanitizer.Sanitize(message);

            var generatedMessage = await this.messageService.CreateAsync(companyId, username, sanitizedMessage, false);

            await this.Clients.Group(generatedMessage.Company.Name)
                      .SendAsync("NewMessage", generatedMessage.MapTo<ChatHubMessageViewModel>());
        }
    }
}