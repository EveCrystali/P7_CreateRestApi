using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Dot.Net.WebApi.Domain
{
    public abstract class DomainBase
    {
        public void Validate()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException($"{this.GetType().Name} is not valid: {errors}");
            }

            // Validate DateTime properties
            ValidateDateTimeProperties();
        }

        private void ValidateDateTimeProperties()
        {
            var dateTimeProperties = this.GetType().GetProperties()
                .Where(prop => prop.PropertyType == typeof(DateTime?));

            foreach (var prop in dateTimeProperties)
            {
                var value = prop.GetValue(this) as DateTime?;
                if (value.HasValue)
                {
                    ValidateDateTime(value, prop.Name);
                }
            }
        }

        private void ValidateDateTime(DateTime? date, string propertyName)
        {
            bool isValid = DateTime.TryParseExact(date.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                                  "yyyy-MM-ddTHH:mm:ss.fffZ",
                                                  CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal,
                                                  out DateTime tempDate);

            if (!isValid)
            {
                throw new ValidationException($"{propertyName} must be in the format yyyy-MM-ddTHH:mm:ss.fffZ");
            }
        }
    }
}