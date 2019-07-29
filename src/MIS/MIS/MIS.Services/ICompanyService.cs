namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    public interface ICompanyService
    {
        Task<CompanyServiceModel> CreateAsync(string name, string address, string userId);

        Task<CompanyServiceModel> CreateAsync(string name, string address);

        Task<CompanyServiceModel> EditAsync(string id, string name, string address);

        Task<CompanyServiceModel> DeleteAsync(string id);

        Task<CompanyServiceModel> GetCompanyAsync(string id);

        Task<CompanyServiceModel> RemoveEmployeeAsync(string id);

        Task<CompanyServiceModel> SetCompanyAsync(Message message, string id);

        Task<CompanyServiceModel> SetCompanyAsync(Report report, string id);

        Task<CompanyServiceModel> SetCompanyAsync(Invitation invitation, string id);

        Task<CompanyServiceModel> SetCompanyAsync(WareHouse wareHouse, string id);

        Task<CompanyServiceModel> SetCompanyAsync(Receipt receipt, string id);
    }
}