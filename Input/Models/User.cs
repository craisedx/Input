using System;
using Microsoft.AspNetCore.Identity;

namespace Input.Models
{
    public class User : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string UserPhoto { get; set; }
    }
}