using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Input.Business.Interfaces;
using Input.Constants.Admin;
using Input.Constants.InfoMessages;
using Input.Email;
using Input.Migrations;
using Input.Models;
using Input.ViewModels;
using Input.ViewModels.Status;
using Input.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Input.Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AdminService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationContext context,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            db = context;
        }

        public IEnumerable<StatusViewModel> GetAllStatuses()
        {
            var statuses = db.Status;

            return mapper.Map<IEnumerable<StatusViewModel>>(statuses);
        }
        
        public IEnumerable<FandomViewModel> GetAllFandoms()
        {
            var fandoms = db.Fandoms;

            return mapper.Map<IEnumerable<FandomViewModel>>(fandoms);
        }

        public FandomViewModel UpdateFandom(FandomViewModel model)
        {
            var updateFandom = db.Fandoms.FirstOrDefault(x => x.Id == model.Id);
            if (updateFandom != null)
            {
                updateFandom.Name = model.Name;
                db.SaveChanges();
            }

            var updatedFandom = db.Fandoms.FirstOrDefault(x => x.Id == updateFandom.Id);

            return mapper.Map<FandomViewModel>(updatedFandom);
        }

        public FandomViewModel CreateFandom(FandomViewModel model)
        {
            var fandom = mapper.Map<Fandom>(model);
            
            if (fandom != null)
            {
                db.Fandoms.Add(fandom);
                db.SaveChanges();
            }

            var addedFandom = db.Fandoms.FirstOrDefault(x => x.Id == fandom.Id);

            return mapper.Map<FandomViewModel>(addedFandom);
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await db.Users.ToListAsync();
            return users;
        }

        public async Task<List<UserViewModel>> GetAllUserWithRoles()
        {
            var userRoles = await db.UserRoles.ToListAsync();
            var usersViewModel = new List<UserViewModel>();
            var adminRoleId = await db.Roles.FirstOrDefaultAsync(x => x.Name == AdminConstants.AdminRole);
            
            foreach (var item in db.Users)
            {
                usersViewModel.Add( new UserViewModel
                {
                    User = item,
                    IsAdmin = userRoles.Any(x => x.RoleId == adminRoleId.Id && x.UserId == item.Id)
                });
            }

            return usersViewModel;
        }

        public async Task BlockUser(string id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return;
            
            await userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddYears(100));
        }
        
        public async Task UnBlockUser(string id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return;
            
            await userManager.SetLockoutEndDateAsync(user, null);
        }
        
        public async Task AddAdmin(string id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return;
            
            await userManager.AddToRoleAsync(user, AdminConstants.AdminRole);
            await signInManager.RefreshSignInAsync(await userManager.FindByIdAsync(id));
        }
        
        public async Task DeleteAdmin(string id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return;

            await userManager.RemoveFromRoleAsync(user, AdminConstants.AdminRole);
            await signInManager.RefreshSignInAsync(await userManager.FindByIdAsync(id));
        }

        public async Task<int> GetCountUsers()
        {
            var countUsers = await db.Users.CountAsync();

            return countUsers;
        }
        
        public async Task<int> GetCountBlockedUsers()
        {
            var countUsers = await db.Users.Where(x => x.LockoutEnd > DateTime.Now).CountAsync();

            return countUsers;
        }
        
        public async Task<int> GetCountUserAdmins()
        {
            var userRoles = await db.UserRoles.ToListAsync();
            var usersViewModel = new List<UserViewModel>();
            
            var adminRoleId = await db.Roles.FirstOrDefaultAsync(x => x.Name == AdminConstants.AdminRole);
            
            foreach (var item in db.Users)
            {
                usersViewModel.Add( new UserViewModel
                {
                    User = item,
                    IsAdmin = userRoles.Any(x => x.RoleId == adminRoleId.Id && x.UserId == item.Id)
                });
                
            }

            var countUsers = usersViewModel.Count(x => x.IsAdmin);

            return countUsers;
        }

        public async Task<List<User>> GetAllAdmins()
        {
            var userRoles = await db.UserRoles.ToListAsync();
            var adminRole = await db.Roles.FirstOrDefaultAsync(x => x.Name == AdminConstants.AdminRole);
            var admins = new List<User>();
            
            foreach (var item in db.Users)
            {
                if (userRoles.Any(x => x.RoleId == adminRole.Id && x.UserId == item.Id))
                {
                    admins.Add(item);
                }
            }
            
            return admins;
        }
    }
}