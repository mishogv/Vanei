namespace MIS.ViewModels.View.Receipt
{
    using System.Collections.Generic;

    using Product;

    public class CreateReceiptViewModel
    {
        public decimal Total { get; set; }

        public string Username { get; set; }

        public IList<ShowReceiptProductViewModel> Products { get; set; }
    }
}