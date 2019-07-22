﻿namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ServicesModels;

    using ViewModels.Input.Category;

    public interface IWareHouseService
    {
        Task<WareHouseServiceModel> CreateAsync(string name, int? companyId);

        Task<WareHouseServiceModel> GetWareHouseByUserNameAsync(string username);

        IEnumerable<WareHouseServiceModel> GetWarehousesByCompanyId(int? companyId);

        Task<WareHouseServiceModel> GetWareHouseByNameAsync(string name);

        IEnumerable<CreateCategoryWareHouseModel> GetAllUserWareHousesByUserName(string username);

        Task<IEnumerable<string>> GetAllCategoriesNamesAsync(string wareHouseName, string username);

        IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int id);
    }
}