namespace MIS.ViewModels.Input.Report
{
    using System;

    public class CreateReportInputModel
    {
        public string CompanyName { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}