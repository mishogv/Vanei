namespace MIS.ViewModels.View.WareHouse
{
    using System.Collections.Generic;

    using Product;

    using Services.Mapping;
    using Services.Models;

    public class IndexWarehouseViewModel : IMapFrom<WareHouseServiceModel>
    {
        public string Id { get; set; }

        public bool IsFavorite { get; set; }
        
        public string WareHouseName { get; set; }
        
        public IEnumerable<WareHouseIndexProductViewModel> Products { get; set; }

        public IEnumerable<IndexWarehouseDropdownViewModel> WarehouseDropdown { get; set; }
    }
}