namespace MIS.ViewModels.View.Category
{
    using Services.Mapping;

    using Services.Models;

    public class CategoryIndexDetailsViewModel : IMapFrom<CategoryServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WarehouseName { get; set; }

        public int ProductsCount { get; set; }
    }
}