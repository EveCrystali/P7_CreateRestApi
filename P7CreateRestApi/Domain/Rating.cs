using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers.Domain
{
    public class Rating
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "MoodysRating is required")]
        [DataType(DataType.Text, ErrorMessage = "MoodysRating must be a string")]
        [MaxLength(10, ErrorMessage = "MoodysRating can't be longer than 10 characters")]
        public required string MoodysRating { get; set; }

        [Required(ErrorMessage = "SandPRating is required")]
        [DataType(DataType.Text, ErrorMessage = "SandPRating must be a string")]
        [MaxLength(10, ErrorMessage = "SandPRating can't be longer than 10 characters")]
        public required string SandPRating { get; set; }

        [Required(ErrorMessage = "FitchRating is required")]
        [DataType(DataType.Text, ErrorMessage = "FitchRating must be a string")]
        [MaxLength(10, ErrorMessage = "FitchRating can't be longer than 10 characters")]
        public required string FitchRating { get; set; }

        [Range(byte.MinValue, byte.MaxValue, ErrorMessage = "OrderNumber must be an integer")]
        public byte? OrderNumber { get; set; }

        public void Validate()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException($"Rating is not valid: {errors}");
            }
        }
    }
}