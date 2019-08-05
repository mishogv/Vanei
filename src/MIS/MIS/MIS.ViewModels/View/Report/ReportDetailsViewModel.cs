namespace MIS.ViewModels.View.Report
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Services.Mapping;

    using Services.Models;

    public class ReportDetailsViewModel : IMapFrom<ReportServiceModel>
    {
        public ReportDetailsViewModel()
        {
            this.Receipts = new List<ReportShowReceiptViewModel>();
        }

        public string Id { get; set; }

        [DisplayName("Username")]
        public string UserUserName { get; set; }

        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public IEnumerable<ReportShowReceiptViewModel> Receipts { get; set; }
    }
}