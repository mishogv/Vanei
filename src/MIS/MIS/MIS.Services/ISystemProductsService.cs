namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface ISystemProductsService
    {
        Task<SystemProduct> CreateSystemProductAsync(string name, decimal price, string imgUrl, string description, string userId);

        Task<SystemProduct> GetSystemProductByIdAsync(int id);

        Task<IEnumerable<SystemProduct>> GetAllSystemProductsAsync();

        Task<bool> ContainsSystemProductAsync(SystemProduct product);

        Task<bool> ContainsSystemProductAsync(int id);

        Task<bool> ContainsSystemProductAsync(string name);

        Task<bool> DeleteSystemProductAsync(int id);

        Task<bool> UpdateSystemProductAsync(int id, string name, decimal price, string imgUrl, string description, string userId);
    }
}