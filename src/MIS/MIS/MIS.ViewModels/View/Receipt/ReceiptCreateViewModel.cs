namespace MIS.ViewModels.View.Receipt
{
    using System.Collections.Generic;

    using Product;

    public class ReceiptCreateViewModel
    {
        public string Total { get; set; }

        public string Username { get; set; }

        public IList<ProductShowReceiptViewModel> Products { get; set; }
    }
}