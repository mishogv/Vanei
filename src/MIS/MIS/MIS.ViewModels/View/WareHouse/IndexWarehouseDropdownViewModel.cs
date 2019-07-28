namespace MIS.ViewModels.View.WareHouse
{
    using Services.Mapping;

    using Services.Models;

    public class IndexWarehouseDropdownViewModel : IMapFrom<WareHouseServiceModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}