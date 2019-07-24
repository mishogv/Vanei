namespace MIS.ViewModels.View.Report
{
    using System;
    using System.Collections.Generic;

    using Services.Mapping;

    using ServicesModels;

    public class DetailsReportViewModel : IMapFrom<ReportServiceModel>
    {
        public DetailsReportViewModel()
        {
            this.Receipts = new List<ShowReceiptReportViewModel>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public IEnumerable<ShowReceiptReportViewModel> Receipts { get; set; }
    }
}