namespace MIS.ViewModels
{
    public class SystemProductShowViewModel
    {
        private string imgUrl;
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl
        {
            get => this.imgUrl;
            set => this.imgUrl = value + ".png" ;
        }

        public string Description { get; set; }
    }
}