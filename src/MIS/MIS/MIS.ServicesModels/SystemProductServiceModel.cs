namespace MIS.ServicesModels
{
    using Models;

    public class SystemProductServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual MISUser User { get; set; }
    }
}