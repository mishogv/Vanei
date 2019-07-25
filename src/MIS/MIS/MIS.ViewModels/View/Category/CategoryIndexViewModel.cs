namespace MIS.ViewModels.View.Category
{
    using System.Collections.Generic;

    using AutoMapper;

    using Services.Mapping;

    using ServicesModels;

    public class CategoryIndexViewModel
    {
        public CategoryIndexViewModel()
        {
            this.Categories= new List<CategoryIndexDetailsViewModel>();
        }

        public IEnumerable<CategoryIndexDetailsViewModel> Categories { get; set; }
    }
}