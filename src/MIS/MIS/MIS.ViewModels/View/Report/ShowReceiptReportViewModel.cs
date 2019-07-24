namespace MIS.ViewModels.View.Report
{
    using System;

    using Models;

    using Services.Mapping;

    public class ShowReceiptReportViewModel : IMapFrom<Receipt>
    {
        public int Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public decimal Total { get; set; }
    }
}