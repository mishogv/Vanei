namespace MIS.ViewModels.View.Category
{
    using System.Collections.Generic;

    public class CategoryIndexViewModel
    {
        public CategoryIndexViewModel()
        {
            this.Categories= new List<CategoryIndexDetailsViewModel>();
        }

        public IEnumerable<CategoryIndexDetailsViewModel> Categories { get; set; }
    }
}