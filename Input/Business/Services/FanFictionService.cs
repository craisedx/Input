using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Input.Business.Interfaces;
using Input.Constants;
using Input.Constants.InfoMessages;
using Input.Constants.Statuses;
using Input.Migrations;
using Input.Models;
using Input.ViewModels;
using Input.ViewModels.Chapter;
using Input.ViewModels.Comment;
using Input.ViewModels.FanFiction;
using Input.ViewModels.Moderation;
using Input.ViewModels.Status;
using Microsoft.EntityFrameworkCore;

namespace Input.Business.Services
{
    public class FanFictionService : IFanFictionService
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;
        
        public FanFictionService(
            ApplicationContext context,
            IMapper mapper)
        {
            this.mapper = mapper;
            db = context;
        }

        public async Task<int> CreateFanFiction(FanFictionViewModel model)
        {
            var createdFanFiction = mapper.Map<FanFiction>(model);
            
            await db.FanFictions.AddAsync(createdFanFiction);
            
            await db.SaveChangesAsync();

            return createdFanFiction.Id;
        }
        
        public async Task AddComment(CommentViewModel model)
        {
            var createdComment = mapper.Map<Comment>(model);
            createdComment.SendingTime = DateTime.Now;
            
            await db.Comments.AddAsync(createdComment);
            
            await db.SaveChangesAsync();
        }

        public async Task<int> AddChapter(ChapterViewModel model)
        {
            var createdChapter = mapper.Map<Chapter>(model);

            await db.Chapters.AddAsync(createdChapter);

            await db.SaveChangesAsync();

            return createdChapter.FanFictionId;
        }
        
        public async Task<string> RemoveChapter(int id)
        {
            var chapter = await db.Chapters.FirstOrDefaultAsync(x => x.Id == id);

            if (chapter == null) return ErrorConstants.ErrorRemoveChapter;

            db.Chapters.Remove(chapter);

            await db.SaveChangesAsync();

            return InfoConstants.SuccessRemoveChapter;
        }
        
        public async Task<string> RemoveFanFiction(int id)
        {
            var fanFiction = await db.FanFictions.FirstOrDefaultAsync(x => x.Id == id);

            if (fanFiction == null) return ErrorConstants.ErrorRemoveFanFiction;

            db.FanFictions.Remove(fanFiction);

            await db.SaveChangesAsync();

            return InfoConstants.SuccessRemoveFanFiction;
        }
        
        public async Task<string> RemoveCommentById(int id)
        {
            var comment = await db.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null) return ErrorConstants.ErrorRemoveComment;

            db.Comments.Remove(comment);

            await db.SaveChangesAsync();

            return InfoConstants.SuccessRemoveComment;
        }
        
        public async Task<int> EditFanFiction(FanFictionViewModel model)
        {
            var editedFanFiction = await db.FanFictions.FirstOrDefaultAsync(x => x.Id == model.Id);

            editedFanFiction.Name = model.Name;
            editedFanFiction.FandomId = model.FandomId;
            editedFanFiction.ShortDescription = model.ShortDescription;
            editedFanFiction.Photo = model.Photo;

            if (editedFanFiction.Moderation != null &&
                editedFanFiction.Moderation.Status.Name != StatusesConstants.BlockedStatus)
                editedFanFiction.Moderation.StatusId = await GetStatusIdByName(StatusesConstants.AwaitProcessingStatus);
            
            await db.SaveChangesAsync();

            return editedFanFiction.Id;
        }

        public async Task<int?> GetPreviousChapterById(int id)
        {
            var chapter = await db.Chapters.FirstOrDefaultAsync(x => x.Id == id);
            
            var allChapters = await db.Chapters.Where(x => x.FanFictionId == chapter.FanFictionId).ToListAsync();

            for (var i = 0; i < allChapters.Count; i++)
            {
                if (chapter != allChapters[i]) continue;
                if (i-1 >= 0) return allChapters[i-1].Id;

                return null;
            }
            
            return null;
        }
        
        public async Task<int?> GetNextChapterById(int id)
        {
            var chapter = await db.Chapters.FirstOrDefaultAsync(x => x.Id == id);
            
            var allChapters = await db.Chapters.Where(x => x.FanFictionId == chapter.FanFictionId).ToListAsync();

            for (var i = 0; i < allChapters.Count; i++)
            {
                if (chapter != allChapters[i]) continue;
                if (i+1 < allChapters.Count) return allChapters[i+1].Id;

                return null;
            }
            
            return null;
        }
        
        public async Task<int> EditChapter(ChapterViewModel model)
        {
            var editedChapter = await db.Chapters.Include(x => x.FanFiction).FirstOrDefaultAsync(x => x.Id == model.Id);

            editedChapter.Name = model.Name;
            editedChapter.ChapterBody = model.ChapterBody;
            editedChapter.Photo = model.Photo;

            await db.SaveChangesAsync();

            return editedChapter.FanFiction.Id;
        }

        public List<PopularFandom> GetBestFandoms(int count)
        {
            var countFanFictionByFandomId = db.FanFictions
                .Include(x => x.Fandom)
                .Include(x => x.UserRating)
                .Include(x => x.User)
                .Where(x => x.Moderation != null && x.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
                .ToList()
                .GroupBy(x => x.Fandom)
                .Select(x =>
                    new PopularFandom()
                    {
                        Fandom = x.Key,
                        Count =  x.Count()
                    });
            
            var fandoms = countFanFictionByFandomId
                .OrderByDescending(x => x.Count)
                .Take(count)
                .ToList();

            return fandoms;
        }

        public List<FanFictionViewModel> GetLastAddedAndApproved(int count)
        {
            var fanFictions = db.FanFictions
                .Include(x => x.Fandom)
                .Include(x => x.User)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Where(x => x.Moderation != null && x.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
                .OrderByDescending(x => x.Moderation.ChangeTime)
                .Take(count)
                .ToList();

            return mapper.Map<List<FanFictionViewModel>>(fanFictions);
        }
        
        public List<UserLikes> GetBestPostByUniqueFandom(int count)
        {
            var countFanFictionByFandomId = db.FanFictions
                .Include(x => x.Fandom)
                .Include(x => x.UserRating)
                .Include(x => x.User)
                .Where(x => x.Moderation != null && x.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
                .ToList()
                .GroupBy(x => x.Fandom)
                .Select(x =>
                    new UserLikes()
                {
                    Fandom = x.Key,
                    FanFiction = x.OrderByDescending(x => x.UserRating.Count).First(),
                });

            var posts = countFanFictionByFandomId
                .OrderByDescending(x => x.FanFiction.UserRating.Count)
                .Take(count)
                .ToList();

            return posts;
        }
        
        public async Task<FanFictionViewModel> GetFanFictionById(int id)
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.UserRating)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Include(x=> x.Moderation)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<FanFictionViewModel>(fanFiction);
        }

        public async Task RemoveLike(int id, string userId)
        {
            var like = await db.UserRatings.FirstOrDefaultAsync(x => x.FanFiction.Id == id && x.User.Id == userId);

            if (like == null) return;

            db.UserRatings.Remove(like);

            await db.SaveChangesAsync();
        }
        
        public async Task SetLike(int id, string userId)
        {
            var like = await db.UserRatings.FirstOrDefaultAsync(x => x.FanFiction.Id == id && x.User.Id == userId);

            if (like != null) return;

            db.UserRatings.Add(new UserRating(){ UserId = userId, FanFictionId = id});

            await db.SaveChangesAsync();
        }
        
        public async Task<string> GetUserPhotoById(string id)
        {
            var user = await db.Users.SingleOrDefaultAsync(u => u.Id == id);

            return user.UserPhoto;
        }
        
        public async Task<List<FanFictionViewModel>> GetActiveFanFictions()
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.UserRating)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Where(x => x.Moderation != null && x.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return mapper.Map<List<FanFictionViewModel>>(fanFiction);
        }
        
        public async Task<List<FanFictionViewModel>> GetActiveFanFictionsByFilters(int? fandomId, string userName)
        {
            var fanFiction = db.FanFictions
                .Include(x => x.UserRating)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Where(x => x.Moderation != null && x.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
                .AsQueryable();

            if (fandomId.HasValue && userName != null)
            {
                fanFiction = fanFiction.OrderByDescending(x => x.User.UserName == userName).ThenByDescending(x => x.Fandom.Id == fandomId.Value);
            }
            else
            {
                if(fandomId.HasValue) fanFiction = fanFiction.OrderByDescending(x => x.Fandom.Id == fandomId.Value);
                
                if (userName != null) fanFiction = fanFiction.OrderByDescending(x => x.User.UserName == userName);
            }

            var getFanFictions = await fanFiction.ToListAsync();
            
            return mapper.Map<List<FanFictionViewModel>>(getFanFictions);
        }
        
        public async Task<List<FanFictionViewModel>> GetActiveFanFictionsByName(string fanFictionName)
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.UserRating)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Where(x => x.Moderation != null 
                            && x.Moderation.Status.Name == StatusesConstants.ApprovedStatus 
                            && x.Name.Contains(fanFictionName))
                .ToListAsync();
            
            return mapper.Map<List<FanFictionViewModel>>(fanFiction);
        }

        public async Task<FandomViewModel> GetFandomById(int id)
        {
            var fandom = await db.Fandoms.FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<FandomViewModel>(fandom);
        }
        
        public async Task<List<FanFictionViewModel>> GetActiveUserFanFictionsById(string id)
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.UserRating)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Where(x => x.User.Id == id && x.Moderation != null && x.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
                .ToListAsync();

            return mapper.Map<List<FanFictionViewModel>>(fanFiction);
        }
        
        public async Task<List<FanFictionViewModel>> GetAwaitingModerationsUserFanFictionsById(string id)
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.UserRating)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.User)
                .Where(x => x.User.Id == id && x.Moderation != null && x.Moderation.Status.Name != StatusesConstants.ApprovedStatus)
                .ToListAsync();

            return mapper.Map<List<FanFictionViewModel>>(fanFiction);
        }
        
        public async Task<List<FanFictionViewModel>> GetAwaitingPublicationsUserFanFictionsById(string id)
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.UserRating)
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Where(x => x.User.Id == id && x.Moderation == null).ToListAsync();

            return mapper.Map<List<FanFictionViewModel>>(fanFiction);
        }
        
        public async Task<List<UserRating>> GetUserLikesById(string id)
        {
            var userLikes = await db.UserRatings
                .Include(x => x.FanFiction)
                .ThenInclude(x => x.User)
                .Include(x => x.FanFiction)
                .ThenInclude(x => x.Fandom)
                .Where(x => x.User.Id == id).ToListAsync();

            return userLikes;
        }
        
        public async Task<List<CommentViewModel>> GetUserCommentsActiveFanFictionsById(string id)
        {
            var userComments = await db.Comments
                .Include(x => x.FanFiction)
                .ThenInclude(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .Where(x => x.User.Id == id 
                            && x.FanFiction.Moderation != null 
                            && x.FanFiction.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
                .OrderByDescending(x => x.SendingTime)
                .ToListAsync();

            return mapper.Map<List<CommentViewModel>>(userComments);
        }
        
        public async Task<CommentViewModel> GetCommentById(int id)
        {
            var userComments = await db.Comments
                .Include(x => x.FanFiction)
                .ThenInclude(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<CommentViewModel>(userComments);
        }

        public async Task<List<ChapterViewModel>> GetChaptersByFanFictionId(int id)
        {
            var chapters = await db.Chapters.Where(x => x.FanFictionId == id).ToListAsync();

            return mapper.Map<List<ChapterViewModel>>(chapters);
        }
        
        public async Task<List<CommentViewModel>> GetCommentsByFanFictionId(int id)
        {
            var chapters = await db.Comments
                .Include(x => x.User)
                .Where(x => x.FanFictionId == id)
                .OrderByDescending(x =>x.SendingTime)
                .ToListAsync();

            return mapper.Map<List<CommentViewModel>>(chapters);
        }
        
        public async Task<User> GetUserById(string id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }
        
        public async Task<ChapterViewModel> GetChapterById(int id)
        {
            var chapters = await db.Chapters
                .Include(x => x.FanFiction)
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<ChapterViewModel>(chapters);
        }
        
        public async Task<List<FandomViewModel>> GetAllFandom()
        {
            var fandoms = await db.Fandoms.ToListAsync();

            return mapper.Map<List<FandomViewModel>>(fandoms);
        }
        
        public async Task<int> GetStatusIdByName(string status)
        {
            var statusDb = await db.Status.FirstOrDefaultAsync(x => x.Name == status);
            return statusDb?.Id ?? 0;
        }

        public async Task<StatusViewModel> GetStatusById(int id)
        {
            var status = await db.Status.FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<StatusViewModel>(status);
        }

        public async Task<string> PublicationFanFiction(int id)
        {
            var fanFiction = await db.FanFictions.Include(x => x.Moderation).FirstOrDefaultAsync(x => x.Id == id && x.Moderation == null);
            var moderation = await CreateModeration();
            
            if(fanFiction== null || moderation == null) return ErrorConstants.ErrorPublicationFanFiction;
            
            fanFiction.ModerationId = moderation.Id;

            await db.SaveChangesAsync();

            return InfoConstants.SuccessSendPublicationFanFiction;
        }

        public async Task<string> SetBlock(int fanFictionId, string userId)
        {
            var fanFiction = await db.FanFictions
                .Include(x => x.Moderation)
                .ThenInclude(x => x.Status)
                .Include(x => x.UserRating)
                .Include(x => x.User)
                .Include(x => x.Fandom)
                .Include(x=> x.Moderation)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == fanFictionId);
            
            var blockStatus = await GetStatusIdByName(StatusesConstants.BlockedStatus);
            if (blockStatus == 0) return InfoConstants.RejectBlockFanFiction;
            
            fanFiction.Moderation.StatusId = blockStatus;
            fanFiction.Moderation.UserId = userId;
            fanFiction.Moderation.Message = InfoConstants.BlockFanFiction;
            fanFiction.Moderation.ChangeTime = DateTime.Now;

            await db.SaveChangesAsync();
            
            return InfoConstants.ApproveBlockFanFiction;
        }

        private async Task<ModerationViewModel> CreateModeration()
        {
            var statusId = await GetStatusIdByName(StatusesConstants.AwaitProcessingStatus);
            if (statusId == 0) return null;
            
            var moderation = new Moderation() { StatusId = statusId};
            
            await db.Moderations.AddAsync(moderation);
            await db.SaveChangesAsync();

            return mapper.Map<ModerationViewModel>(moderation);
        }
    }
}