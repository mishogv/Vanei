namespace MIS.ViewModels.Input.Category
{
    using Models;

    using Services.Mapping;

    public class CreateCategoryWareHouseModel : IMapFrom<WareHouse>
    {
        public string Name { get; set; }
    }
}