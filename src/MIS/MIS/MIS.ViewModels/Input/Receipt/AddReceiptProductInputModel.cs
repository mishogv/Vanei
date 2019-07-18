namespace MIS.ViewModels.Input.Receipt
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Models;

    public class AddReceiptProductInputModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }


        [Range(0.001, double.MaxValue)]
        public double Quantity { get; set; }
    }
}