namespace MIS.ViewModels.View.Report
{
    using System.Collections.Generic;

    public class ReportIndexViewModel
    {
        public ReportIndexViewModel()
        {
            this.Reports = new HashSet<ReportIndexShowViewModel>();
        }

        public IEnumerable<ReportIndexShowViewModel> Reports { get; set; }
    }
}