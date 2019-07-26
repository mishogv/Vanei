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
            var userId = this.userManager.GetUserId(this.User);

            var invitations = await this.invitationService.GetAllAsync(userId);

            var result = new InvitationIndexViewModel
            {
                Invitations = invitations.Select(x => x.MapTo<InvitationIndexDetailsViewModel>()).ToList(),
            };

            return this.View(result);
        }

        public async Task<IActionResult> Accept(int id)
        {
            await this.invitationService.AcceptInvitationAsync(id);

            return this.RedirectToAction("Index", "Company");
        }

        public async Task<IActionResult> Decline(int id)
        {
            await this.invitationService.DeclineInvitationAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}