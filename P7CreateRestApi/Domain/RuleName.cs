using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Controllers
{
    public class RuleName : IValidatable
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text, ErrorMessage = "Name must be a string")]
        [MaxLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.MultilineText, ErrorMessage = "Description must be a string")]
        [MaxLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Json is required")]
        [DataType(DataType.MultilineText, ErrorMessage = "Json must be a string")]
        [MaxLength(5000, ErrorMessage = "Json can't be longer than 5000 characters")]
        public required string Json { get; set; }

        [Required(ErrorMessage = "Template is required")]
        [DataType(DataType.MultilineText, ErrorMessage = "Template must be a string")]
        [MaxLength(1000, ErrorMessage = "Template can't be longer than 1000 characters")]
        public required string Template { get; set; }

        [Required(ErrorMessage = "SqlStr is required")]
        [DataType(DataType.MultilineText, ErrorMessage = "SqlStr must be a string")]
        [MaxLength(1000, ErrorMessage = "SqlStr can't be longer than 1000 characters")]
        public required string SqlStr { get; set; }

        [Required(ErrorMessage = "SqlPart is required")]
        [DataType(DataType.MultilineText, ErrorMessage = "SqlPart must be a string")]
        [MaxLength(1000, ErrorMessage = "SqlPart can't be longer than 1000 characters")]
        public required string SqlPart { get; set; }

        public void Validate()
        {
            ValidationExtensions.Validate(this);
        }
    }
}