﻿using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is mandatory")]
        [DataType(DataType.EmailAddress)]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Full name is mandatory")]
        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage = "Fullname must be at most 100 characters long.")]
        public required string Fullname { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_\W]).+$", ErrorMessage = "The password must contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        public required string Password { get; set; }
    }
}