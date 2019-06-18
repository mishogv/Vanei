namespace MIS.WebApp.Areas.Administrator.BindingModels
{
    public class SystemProductDetailsBindingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl { get; set; }

        public string Description { get; set; }
    }
}