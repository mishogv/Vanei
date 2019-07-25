namespace MIS.ViewModels.View.Category
{
    using Services.Mapping;

    using ServicesModels;

    public class CategoryIndexDetailsViewModel : IMapFrom<CategoryServiceModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string WarehouseName { get; set; }

        public int ProductsCount { get; set; }
    }
}