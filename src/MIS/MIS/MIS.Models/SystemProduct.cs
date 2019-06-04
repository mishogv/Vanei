namespace MIS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SystemProduct : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual MISUser User { get; set; }
    }
}