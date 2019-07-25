namespace MIS.WebApp.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.View.Invitation;

    public class InvitationController : AuthenticationController
    {
        private readonly UserManager<MISUser> userManager;
        private readonly IInvitationService invitationService;

        public InvitationController(UserManager<MISUser> userManager,
            IInvitationService invitationService)
        {
            this.userManager = userManager;
            this.invitationService = invitationService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var invitations = await this.invitationService.GetAllAsync(user.Id);

            var result = new InvitationIndexViewModel
            {
                Invitations = invitations.Select(x => x.MapTo<InvitationIndexDetailsViewModel>()).ToList(),
            };

            return this.View(result);
        }
    }
}