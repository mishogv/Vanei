namespace MIS.ViewModels.Input.Category
{
    using Models;

    using Services.Mapping;

    public class CreateCategoryWareHouseModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public int Id { get; set; }
    }
}