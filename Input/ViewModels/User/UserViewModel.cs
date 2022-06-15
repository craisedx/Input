using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Input.ViewModels.User
{
    public class UserViewModel
    {
        public Models.User User { get; set; }
        public bool IsAdmin { get; set; }
    }
}