using System.Collections.Generic;
using System.Threading.Tasks;
using Input.Models;
using Input.ViewModels;
using Input.ViewModels.Status;
using Input.ViewModels.User;

namespace Input.Business.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<StatusViewModel> GetAllStatuses();
        IEnumerable<FandomViewModel> GetAllFandoms();
        FandomViewModel UpdateFandom(FandomViewModel model);
        FandomViewModel CreateFandom(FandomViewModel model);
        Task<List<User>> GetAllUsers();
        Task<List<UserViewModel>> GetAllUserWithRoles();
        Task BlockUser(string id);
        Task UnBlockUser(string id);
        Task AddAdmin(string id);
        Task DeleteAdmin(string id);
        Task<int> GetCountUsers();
        Task<int> GetCountBlockedUsers();
        Task<int> GetCountUserAdmins();
        Task<List<User>> GetAllAdmins();
    }
}