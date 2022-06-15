using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Input.Business.Interfaces;
using Input.Constants.Admin;
using Input.Migrations;
using Input.Models;
using Input.ViewModels.Account;
using Microsoft.EntityFrameworkCore;

namespace Input.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;
        
        public UserService(
            ApplicationContext context,
            IMapper mapper)
        {
            this.mapper = mapper;
            db = context;
        }
        
        public async Task<User> GetUserById(string id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }
        
        public async Task<EditProfileViewModel> GetEditUserById(string id)
        {
            var editUser = await db.Users.FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<EditProfileViewModel>(editUser);
        }

        public async Task<bool> UserIsAdminById(string id)
        {
            var adminRoleId = await db.Roles.FirstOrDefaultAsync(x => x.Name == AdminConstants.AdminRole);
            var userIsAdmin = db.UserRoles.Any(x => x.RoleId == adminRoleId.Id && x.UserId == id);
            
            return userIsAdmin;
        }

        public async Task<string> EditProfile(EditProfileViewModel model)
        {
            var editedProfile = await db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (editedProfile == null) return null;

            editedProfile.UserPhoto = model.Photo;

            await db.SaveChangesAsync();
            
            return editedProfile.Id;
        }

        public async Task SetLastLogin(LoginViewModel model)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            
            if (user == null) return;
            
            user.LastLoginDate = DateTime.Now;

            await db.SaveChangesAsync();
        }
    }
}