namespace MIS.ViewModels.View.WareHouse
{
    using System.Collections.Generic;

    using Product;

    using Services.Mapping;
    using Services.Models;

    public class WarehouseIndexViewModel : IMapFrom<WareHouseServiceModel>
    {
        public string Id { get; set; }

        public bool IsFavorite { get; set; }
        
        public string Name { get; set; }
        
        public IEnumerable<ProductWareHouseIndexViewModel> Products { get; set; }

        public IEnumerable<WarehouseIndexDropdownViewModel> WarehouseDropdown { get; set; }
    }
}