using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class CurvePoint
    {
        public int Id { get; set; }

        [Range(byte.MinValue, byte.MaxValue, ErrorMessage = "CurveId must be an integer")]
        public byte? CurveId { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "AsOfDate must be a date and a time of day")]
        public DateTime? AsOfDate { get; set; }

        [DoubleValidation(ErrorMessage = "Term must be a valid double")]
        public double? Term { get; set; }

        [DoubleValidation(ErrorMessage = "CurvePointValue must be a valid double")]
        public double? CurvePointValue { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "CreationDate must be a date and a time of day")]
        public DateTime? CreationDate { get; set; }

        public void Validate()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException($"CurvePoint is not valid: {errors}");
            }
        }
    }
}