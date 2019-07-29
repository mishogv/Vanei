namespace MIS.Services
{
    using System.Threading.Tasks;

    using MIS.Models;

    public interface IUserService
    {
        Task AddToCompanyAsync(Company company, string id);
    }
}