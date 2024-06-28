using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public static class UserExtensions
    {
        public static bool IsUserActive(this User user)
        {
            if (user.LastLoginDate == null)
            {
                return false;
            }

            return user.LastLoginDate >= DateTime.UtcNow.AddYears(-2);
        }
    }
}