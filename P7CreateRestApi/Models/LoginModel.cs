﻿using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "ErrorMissingUsername")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Error Invalid Email address")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "ErrorMissingPassword")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}