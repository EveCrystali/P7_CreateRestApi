using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
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