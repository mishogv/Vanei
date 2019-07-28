namespace MIS.ViewModels.View.Report
{
    using System;

    using Services.Mapping;

    using Services.Models;

    public class IndexReportShowViewModel : IMapFrom<ReportServiceModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string CompanyName { get; set; }

        public string UserUsername { get; set; }
    }
}