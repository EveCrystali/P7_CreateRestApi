using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
    }
}