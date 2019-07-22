namespace MIS.ViewModels.View.Report
{
    using System;

    using Services.Mapping;

    using ServicesModels;

    public class IndexReportViewModel : IMapFrom<ReportServiceModel>
    {
        public int Id { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string CompanyName { get; set; }

        public string UserUsername { get; set; }
    }
}