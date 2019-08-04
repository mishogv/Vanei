namespace MIS.ViewModels.View.Report
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Models;

    using Services.Mapping;
    using Services.Models;

    public class ShowReceiptReportViewModel : IMapFrom<Receipt>
    {
        public int Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public decimal Total { get; set; }
    }
}