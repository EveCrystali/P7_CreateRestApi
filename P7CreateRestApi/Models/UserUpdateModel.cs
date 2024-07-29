using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Models;

public class UserUpdateModel
{
    [Required(ErrorMessage = "Id is mandatory")]
    public required string Id { get; set; }

    [Required(ErrorMessage = "Full name is mandatory")]
    [DataType(DataType.Text)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name should contain only letters and spaces")]
    [StringLength(25, MinimumLength = 5, ErrorMessage = "Full name should be between 5 and 25 characters long")]
    public required string Fullname { get; set; }

    [Required(ErrorMessage = "Email is mandatory")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [RegularExpression(@"^(User|Trader)$", ErrorMessage = "Invalid role. Role must be 'User' or 'Trader'.")]
    public string? Role { get; set; }
}