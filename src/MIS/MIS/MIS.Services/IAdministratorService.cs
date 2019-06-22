namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    public interface IAdministratorService
    {
        Task<bool> CreateAdministratorByIdAsync(string id);

        Task<bool> DropAdministratorByIdAsync(string id);

        Task<string> GetUserRoleAsync(MISUser user);

        IQueryable<MISUser> GetAllUsers();
    }
}