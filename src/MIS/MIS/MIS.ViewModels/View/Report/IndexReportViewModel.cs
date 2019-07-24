namespace MIS.ViewModels.View.Report
{
    using System;
    using System.Collections.Generic;

    using Services.Mapping;

    using ServicesModels;

    public class IndexReportViewModel
    {
        public IndexReportViewModel()
        {
            this.Reports = new HashSet<IndexReportShowViewModel>();
        }

        public IEnumerable<IndexReportShowViewModel> Reports { get; set; }
    }
}