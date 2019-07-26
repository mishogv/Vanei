namespace MIS.WebApp.Areas.Owner.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;

    using WebApp.Controllers;

    [Area(GlobalConstants.CompanyOwnerRole)]
    [Authorize(Roles = GlobalConstants.CompanyOwnerRole)]
    public class InvitationController : AuthenticationController
    {
        private readonly IInvitationService invitationService;
        private readonly UserManager<MISUser> userManager;

        public InvitationController(IInvitationService invitationService,
            UserManager<MISUser> userManager)
        {
            this.invitationService = invitationService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Search()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var users = await this.invitationService.GetAllUsersAsync();

            foreach (var user in users)
            {
                if (user.CompanyName == null)
                {
                    user.CompanyName = GlobalConstants.NotApplicable;
                    user.IsAvailableForInvite = true;

                    if (user.Invitations.Select(x => x.CompanyId).Contains((int) currentUser.CompanyId))
                    {
                        user.IsAvailableForInvite = false;
                    }
                }
                else if (user.CompanyId == currentUser.CompanyId)
                {
                    user.IsAvailableForInvite = false; 
                }
            }

            return this.View(users);
        }

        public async Task<IActionResult> Invite(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.invitationService.InviteAsync(user.CompanyId, id);

            return this.RedirectToAction(nameof(this.Search));
        }
    }
}