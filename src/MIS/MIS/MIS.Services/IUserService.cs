namespace MIS.Services
{
    using System.Threading.Tasks;

    using Models;

    public interface IUserService
    {
        //Task<int?> GetUserWarehouseIdAsync(string username);
        Task<MISUser> GetUserByUsernameAsync(string username);
    }
}