using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Input.ViewModels.FanFiction;
using Input.ViewModels.Moderation;
using Input.ViewModels.Status;

namespace Input.Business.Interfaces
{
    public interface IModerationService
    {
        Task<List<FanFictionViewModel>> GetAwaitingModerationFanFiction();
        bool CheckAccessToViewPost(ClaimsPrincipal user, FanFictionViewModel fanFiction);
        Task<List<StatusViewModel>> GetAllStatuses();
        Task CheckedFanFiction(ModerationViewModel moderation);
        Task<string> SetAdminToModeration(int id, string userId);
        Task<List<FanFictionViewModel>> GetActiveAdminModerationFanFiction(ClaimsPrincipal user);
        Task<int> GetFanFictionIdByModerationId(int id);
        Task RemoveAdminToModeration(int id);
        Task SetStatusAwaitProcessed(int id);
        Task<List<FanFictionViewModel>> GetAllFanFictions();
    }
}