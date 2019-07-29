namespace MIS.WebApp.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Data;

    using Microsoft.Extensions.DependencyInjection;

    public class UsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var db = validationContext.GetService<MISDbContext>();
            var isAvailable = !db.Users.Select(x => x.UserName).Contains(value.ToString());

            if (!isAvailable)
            {
                return new ValidationResult("Username is already used.");
            }

            return ValidationResult.Success;
        }
    }
}