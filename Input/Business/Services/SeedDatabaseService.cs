using System;
using System.Linq;
using System.Threading.Tasks;
using Input.Business.Interfaces;
using Input.Constants.Admin;
using Input.Constants.Statuses;
using Input.Constants.User;
using Input.Migrations;
using Input.Models;
using Microsoft.AspNetCore.Identity;

namespace Input.Business.Services
{
    public class SeedDatabaseService : ISeedDatabaseService
    {
        private readonly ApplicationContext db;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public SeedDatabaseService(ApplicationContext context, RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            db = context;
        }
        
        public async Task CreateStartAdmin()
        {
            if (db.Users.Any(x => x.UserName == AdminConstants.AdminAccountName))
            {
                Console.WriteLine(AdminConstants.AdminAccountAvailable);
            }
            else
            {
                var user = new User
                {
                    Email = AdminConstants.AdminAccountEmail, UserName = AdminConstants.AdminRole,
                    UserPhoto = UserConstants.DefaultUserPhoto
                };

                user.EmailConfirmed = true;
                await userManager.CreateAsync(user, AdminConstants.AdminPassword);
                await db.SaveChangesAsync();
                await userManager.AddToRoleAsync(user, AdminConstants.AdminRole);
                Console.WriteLine(AdminConstants.AdminCreated);
            }
        }
        
        public void CreateStartStatuses()
        {
            if (db.Status.Any(x => x.Name == StatusesConstants.AwaitProcessingStatus))
            {
                Console.WriteLine(StatusesConstants.StatusAvailable(StatusesConstants.AwaitProcessingStatus));
            }
            else
            {
                db.Status.Add(new Status
                { 
                    Name = StatusesConstants.AwaitProcessingStatus
                });
                db.SaveChanges();
                
                Console.WriteLine(StatusesConstants.StatusCreated(StatusesConstants.AwaitProcessingStatus));
            }

            if (db.Status.Any(x => x.Name == StatusesConstants.ProcessedStatus))
            {
                Console.WriteLine(StatusesConstants.StatusAvailable(StatusesConstants.ProcessedStatus));
            }
            else
            {
                db.Status.Add(new Status
                {
                    Name = StatusesConstants.ProcessedStatus
                });
                db.SaveChanges();
                
                Console.WriteLine(StatusesConstants.StatusCreated(StatusesConstants.ProcessedStatus));
            }

            if (db.Status.Any(x => x.Name == StatusesConstants.RejectedStatus))
            {
                Console.WriteLine(StatusesConstants.StatusAvailable(StatusesConstants.RejectedStatus));
            }
            else
            {
                db.Status.Add(new Status
                {
                    Name = StatusesConstants.RejectedStatus
                });
                db.SaveChanges();
                
                Console.WriteLine(StatusesConstants.StatusCreated(StatusesConstants.RejectedStatus));
            }
            
            if (db.Status.Any(x => x.Name == StatusesConstants.ApprovedStatus))
            {
                Console.WriteLine(StatusesConstants.StatusAvailable(StatusesConstants.ApprovedStatus));
            }
            else
            {
                db.Status.Add(new Status
                {
                    Name = StatusesConstants.ApprovedStatus
                });
                db.SaveChanges();
                
                Console.WriteLine(StatusesConstants.StatusCreated(StatusesConstants.ApprovedStatus));
            }
            
            if (db.Status.Any(x => x.Name == StatusesConstants.BlockedStatus))
            {
                Console.WriteLine(StatusesConstants.StatusAvailable(StatusesConstants.BlockedStatus));
            }
            else
            {
                db.Status.Add(new Status
                {
                    Name = StatusesConstants.BlockedStatus
                });
                db.SaveChanges();
                
                Console.WriteLine(StatusesConstants.StatusCreated(StatusesConstants.BlockedStatus));
            }
        }
        
        public async Task CreateStartRoles()
        {
            if (db.Roles.Any(x => x.Name == AdminConstants.AdminRole))
            {
                Console.WriteLine(AdminConstants.AdminRoleAvailable(AdminConstants.AdminRole));
            }
            else
            {
                await roleManager.CreateAsync(new IdentityRole(AdminConstants.AdminRole));
                Console.WriteLine(AdminConstants.AdminRoleCreated(AdminConstants.AdminRole));
            }
        }
    }
}