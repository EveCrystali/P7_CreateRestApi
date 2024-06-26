﻿using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "ErrorMissingUsername")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "ErrorMissingPassword")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W]).+$", ErrorMessage = "The password must contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}