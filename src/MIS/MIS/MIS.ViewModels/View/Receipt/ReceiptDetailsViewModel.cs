namespace MIS.ViewModels.View.Receipt
{
    using System;
    using System.Collections.Generic;

    using Services.Mapping;

    using Services.Models;

    public class ReceiptDetailsViewModel : IMapFrom<ReceiptServiceModel>
    {
        public int Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public string Username { get; set; }

        public IEnumerable<ReceiptProductDetailsViewModel> Products { get; set; }
    }
}