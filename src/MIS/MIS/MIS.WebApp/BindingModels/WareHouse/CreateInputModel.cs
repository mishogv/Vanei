﻿namespace MIS.WebApp.BindingModels.WareHouse
{
    using System.ComponentModel.DataAnnotations;

    public class CreateInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }
    }
}