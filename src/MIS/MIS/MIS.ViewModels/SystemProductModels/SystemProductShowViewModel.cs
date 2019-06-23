namespace MIS.ViewModels.SystemProductModels
{
    public class SystemProductShowViewModel
    {
        private const string ExtensionPng = ".png";

        private string imgUrl;

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl
        {
            get => this.imgUrl;
            set => this.imgUrl = value + ExtensionPng ;
        }

        public string Description { get; set; }
    }
}