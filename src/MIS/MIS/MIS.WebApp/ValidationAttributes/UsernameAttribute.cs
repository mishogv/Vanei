namespace MIS.WebApp.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Data;

    public class UsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var db = (MISDbContext) validationContext.GetService(typeof(MISDbContext));
            var isAvailable = !db.Users.Select(x => x.UserName).Contains(value.ToString());

            if (isAvailable)
            {
                return base.IsValid(value, validationContext);
            }

            return new ValidationResult("Username is already used.");
        }
    }
}