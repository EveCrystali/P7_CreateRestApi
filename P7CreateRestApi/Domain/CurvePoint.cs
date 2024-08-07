using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class CurvePoint : IValidatable
    {
        public int Id { get; set; }

        [Range(byte.MinValue, byte.MaxValue, ErrorMessage = "CurveId must be an integer")]
        public byte? CurveId { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "AsOfDate must be a date and a time of day")]
        public DateTime? AsOfDate { get; set; }

        [DoubleNullableValidation(ErrorMessage = "Term must be a valid double")]
        public double? Term { get; set; }

        [DoubleNullableValidation(ErrorMessage = "CurvePointValue must be a valid double")]
        public double? CurvePointValue { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "CreationDate must be a date and a time of day")]
        public DateTime? CreationDate { get; set; }

        public void Validate()
        {
            ValidationExtensions.Validate(this);
        }
    }
}