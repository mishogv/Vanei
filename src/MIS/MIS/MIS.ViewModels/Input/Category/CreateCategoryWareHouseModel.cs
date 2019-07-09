namespace MIS.ViewModels.Input.Category
{
    using System.Collections.Generic;

    using Models;

    using Services.Mapping;

    using View.Product;

    public class CreateCategoryWareHouseModel : IMapFrom<WareHouse>
    {
        public string Name { get; set; }

        public ICollection<WareHouseIndexProductViewModel> Products { get; set; }
    }
}