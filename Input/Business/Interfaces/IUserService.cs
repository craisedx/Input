using System.Collections.Generic;
using System.Threading.Tasks;
using Input.Models;
using Input.ViewModels.Account;
using Input.ViewModels.User;

namespace Input.Business.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(string id);
        Task SetLastLogin(LoginViewModel model);
        Task<EditProfileViewModel> GetEditUserById(string id);
        Task<string> EditProfile(EditProfileViewModel model);
        Task<bool> UserIsAdminById(string id);
    }
}