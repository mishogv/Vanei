namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using MIS.Models;


    public interface IAdministratorService
    {
        Task<bool> CreateAdministratorByIdAsync(string id);

        Task<bool> RemoveAdministratorByIdAsync(string id);

        Task<string> GetUserRoleAsync(MISUser user);

        IQueryable<MISUser> GetAllUsers();
    }
}