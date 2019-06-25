﻿namespace MIS.WebApp.BindingModels.Category
{
    using System.ComponentModel.DataAnnotations;

    public class CreateInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 2)]
        public string Name { get; set; }
    }
}