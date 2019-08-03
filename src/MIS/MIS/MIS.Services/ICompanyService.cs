namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using MIS.Models;
    using MIS.Models.Contracts;

    using Models;

    public interface ICompanyService
    {
        Task<CompanyServiceModel> CreateAsync(string name, string address, string userId);

        Task<CompanyServiceModel> CreateAsync(string name, string address);

        Task<CompanyServiceModel> EditAsync(string id, string name, string address);

        Task<CompanyServiceModel> DeleteAsync(string id);

        Task<CompanyServiceModel> GetCompanyAsync(string id);

        Task<CompanyServiceModel> RemoveEmployeeAsync(string id);

        Task<CompanyServiceModel> SetCompanyAsync(IHaveCompany obj, string id);

        Task<CompanyServiceModel> SetCompanyAsync(WareHouse wareHouse, string id);
    }
}