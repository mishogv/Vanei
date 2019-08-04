namespace MIS.ViewModels.View.Report
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Services.Mapping;

    using Services.Models;

    public class DetailsReportViewModel : IMapFrom<ReportServiceModel>
    {
        public DetailsReportViewModel()
        {
            this.Receipts = new List<ShowReceiptReportViewModel>();
        }

        public string Id { get; set; }

        [DisplayName("Username")]
        public string UserUserName { get; set; }

        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public IEnumerable<ShowReceiptReportViewModel> Receipts { get; set; }
    }
}