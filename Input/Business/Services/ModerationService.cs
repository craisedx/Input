using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Input.Business.Interfaces;
using Input.Constants;
using Input.Constants.Admin;
using Input.Constants.InfoMessages;
using Input.Constants.Statuses;
using Input.Migrations;
using Input.ViewModels.FanFiction;
using Input.ViewModels.Moderation;
using Input.ViewModels.Status;
using Microsoft.EntityFrameworkCore;

namespace Input.Business.Services
{
    public class ModerationService : IModerationService
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;
        private readonly IFanFictionService fanFictionService;
        public ModerationService(
            ApplicationContext context,
            IMapper mapper,
            IFanFictionService fanFictionService)
        {
            this.fanFictionService = fanFictionService;
            this.mapper = mapper;
            db = context;
        }

        public async Task<List<FanFictionViewModel>> GetAwaitingModerationFanFiction()
        {
            var fanFictions = await db.FanFictions
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Where(x => x.Moderation.Status.Name == StatusesConstants.AwaitProcessingStatus && x.Moderation.User == null).ToListAsync();
            
            return mapper.Map<List<FanFictionViewModel>>(fanFictions);
        }

        public async Task<List<FanFictionViewModel>> GetActiveAdminModerationFanFiction(ClaimsPrincipal user)
        {
            var userId = user.Claims.ElementAt(0).Value;
            var userIsAdmin = user.IsInRole(AdminConstants.AdminRole);

            if (string.IsNullOrEmpty(userId) && !userIsAdmin) return null;
                
            var fanFictions = await db.FanFictions
                .Include(x => x.Fandom)
                .Include(x => x.User)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.User)
                .Where(x => x.Moderation.User.Id == userId 
                            && x.Moderation.Status.Name != StatusesConstants.BlockedStatus)
                .OrderByDescending(x => x.Moderation.ChangeTime)
                .ToListAsync();
            
            return mapper.Map<List<FanFictionViewModel>>(fanFictions);
        }
        
        public async Task<List<FanFictionViewModel>> GetAllFanFictions()
        {
            var fanFictions = await db.FanFictions
                .Include(x => x.Fandom)
                .Include(x => x.User)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.User)
                .OrderByDescending(x => x.Moderation.ChangeTime)
                .ToListAsync();
            
            return mapper.Map<List<FanFictionViewModel>>(fanFictions);
        }

        public async Task SetStatusAwaitProcessed(int id)
        {
            var moderation = await db.Moderations.FirstOrDefaultAsync(x => x.Id == id);
            if (moderation == null) return;
            
            var statusId = await fanFictionService.GetStatusIdByName(StatusesConstants.ProcessedStatus);
            if (statusId == 0) return;
            
            moderation.StatusId = statusId;

            await db.SaveChangesAsync();
        }
        
        public bool CheckAccessToViewPost(ClaimsPrincipal user, FanFictionViewModel fanFiction)
        {
            if (user.Identity.IsAuthenticated)
            {
                var userId = user.Claims.ElementAt(0).Value;
                var userIsAdmin = user.IsInRole(AdminConstants.AdminRole);

                return userId == fanFiction.UserId ||
                       fanFiction.Moderation != null && fanFiction.Moderation.Status.Name == StatusesConstants.ApprovedStatus ||
                       userIsAdmin;
            }

            return fanFiction.Moderation != null && fanFiction.Moderation.Status.Name == StatusesConstants.ApprovedStatus;
        }

        public async Task<List<StatusViewModel>> GetAllStatuses()
        {
            var statuses = await db.Status.ToListAsync();

            return mapper.Map<List<StatusViewModel>>(statuses);
        }

        public async Task CheckedFanFiction(ModerationViewModel moderation)
        {
            var moderationEdit = await db.Moderations.FirstOrDefaultAsync(x => x.Id == moderation.Id);
            
            if (moderationEdit == null) return;

            moderationEdit.Message = moderation.Message;
            moderationEdit.StatusId = moderation.StatusId;
            moderationEdit.ChangeTime = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task<int> GetFanFictionIdByModerationId(int id)
        {
            var fanFiction = await db.FanFictions.Include(x => x.Moderation)
                .FirstOrDefaultAsync(x => x.Moderation.Id == id);

            return fanFiction?.Id ?? 0;
        }

        public async Task RemoveAdminToModeration(int id)
        {
            var moderation = await db.Moderations.FirstOrDefaultAsync(x => x.Id == id);
            if (moderation == null) return;

            moderation.UserId = null;

            await db.SaveChangesAsync();
        }
        
        public async Task<string> SetAdminToModeration(int id, string userId)
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id && x.Moderation.User == null);
            
            var statusId = await fanFictionService.GetStatusIdByName(StatusesConstants.ProcessedStatus);

            if (fanFiction == null || statusId == 0) return ErrorConstants.ErrorAddAdminToFanFictionModeration;

            fanFiction.Moderation.UserId = userId;
            fanFiction.Moderation.StatusId = statusId;
            fanFiction.Moderation.ChangeTime = DateTime.Now;

            await db.SaveChangesAsync();
            
            return InfoConstants.SuccessAddAdminToFanFictionModeration;
        }
    }
}