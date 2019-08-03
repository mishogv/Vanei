namespace MIS.WebApp.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.View.Invitation;

    public class InvitationController : AuthenticationController
    {
        private readonly UserManager<MISUser> userManager;
        private readonly SignInManager<MISUser> signInManager;
        private readonly IInvitationService invitationService;

        public InvitationController(UserManager<MISUser> userManager,
            IInvitationService invitationService,
            SignInManager<MISUser> signInManager)
        {
            this.userManager = userManager;
            this.invitationService = invitationService;
            this.signInManager = signInManager;
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

        public async Task<IActionResult> Accept(string id)
        {
            var isOwner = this.User.IsInRole(GlobalConstants.CompanyOwnerRole);

            await this.invitationService.AcceptInvitationAsync(id, isOwner);

            if (isOwner)
            {
                var user = await this.userManager.GetUserAsync(this.User);

                await this.signInManager.SignOutAsync();
                await this.signInManager.SignInAsync(user, false);
            }

            return this.RedirectToAction("Index", "Company");
        }

        public async Task<IActionResult> Decline(string id)
        {
            await this.invitationService.DeclineInvitationAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}