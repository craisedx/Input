using Microsoft.AspNetCore.Identity;

namespace Input.Models
{
    public class User : IdentityUser
    {
        public string UserPhoto { get; set; }
    }
}