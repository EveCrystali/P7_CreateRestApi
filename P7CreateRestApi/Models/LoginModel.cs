using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "ErrorMissingUsername")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required(ErrorMessage = "ErrorMissingPassword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}