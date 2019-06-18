namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using ServicesModels;

    public interface ISystemProductsService
    {
        Task<SystemProductServiceModel> CreateSystemProductAsync(string name, decimal price, string imgUrl, string description, string userId);

        Task<SystemProductServiceModel> GetSystemProductByIdAsync(int id);

        IQueryable<SystemProductServiceModel> GetAllSystemProducts();

        //Task<bool> ContainsSystemProductAsync(SystemProduct product);

        //Task<bool> ContainsSystemProductAsync(int id);

        //Task<bool> ContainsSystemProductAsync(string name);

        Task<bool> DeleteSystemProductAsync(int id);

        Task<bool> UpdateSystemProductByIdAsync(int id, string name, decimal price, string imgUrl, string description);
    }
}