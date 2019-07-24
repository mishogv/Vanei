namespace MIS.ViewModels.Input.Report
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateReportInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime From { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime To { get; set; } = DateTime.UtcNow;
    }
}