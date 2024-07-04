using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Full name is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name should contain only letters and spaces")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Full name should be between 5 and 25 characters long")]
        public required string Fullname { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "LastLoginDate must be a date and a time of day")]
        public DateTime? LastLoginDate { get; set; }

        public void Validate()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException($"BidList is not valid: {errors}");
            }
        }

    }

    public static class UserExtensions
    {
        public static bool IsUserActive(this User user)
        {
            if (user.LastLoginDate == null)
            {
                return false;
            }

            return user.LastLoginDate >= DateTime.UtcNow.AddYears(-2);
        }
    }
}