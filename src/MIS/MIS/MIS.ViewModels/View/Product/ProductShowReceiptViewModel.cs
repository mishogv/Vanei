namespace MIS.ViewModels.View.Product
{
    using AutoMapper;

    using Models;

    using Services.Mapping;
    using Services.Models;

    public class ProductShowReceiptViewModel : IMapFrom<ReceiptProduct>, IMapFrom<ReceiptProductServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public double Quantity { get; set; }

        public decimal Total { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductBarcode { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ReceiptProduct, ProductShowReceiptViewModel>()
                .ForMember(destination => destination.Total,
                    opts => opts.MapFrom(origin
                        => origin.Product.Price * (decimal) origin.Quantity));
        }
    }
}