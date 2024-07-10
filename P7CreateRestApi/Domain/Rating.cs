using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Controllers.Domain
{
    public class Rating : IValidatable
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
            ValidationExtensions.Validate(this);
        }
    }
}