using System.Collections.Generic;
using System.Threading.Tasks;
using Input.Models;
using Input.ViewModels;
using Input.ViewModels.Chapter;
using Input.ViewModels.Comment;
using Input.ViewModels.FanFiction;
using Input.ViewModels.Status;

namespace Input.Business.Interfaces
{
    public interface IFanFictionService
    {
        Task<List<FandomViewModel>> GetAllFandom();
        Task<int> CreateFanFiction(FanFictionViewModel model);
        Task<FanFictionViewModel> GetFanFictionById(int id);
        Task<int> GetStatusIdByName(string status);
        Task<int> EditFanFiction(FanFictionViewModel model);
        Task<int> AddChapter(ChapterViewModel model);
        Task<int> EditChapter(ChapterViewModel model);
        Task<string> RemoveChapter(int id);
        Task<string> RemoveFanFiction(int id);
        Task<List<ChapterViewModel>> GetChaptersByFanFictionId(int id);
        Task<List<CommentViewModel>> GetCommentsByFanFictionId(int id);
        Task<ChapterViewModel> GetChapterById(int id);
        Task<string> PublicationFanFiction(int id);
        Task<List<FanFictionViewModel>> GetActiveUserFanFictionsById(string id);
        Task<List<FanFictionViewModel>> GetAwaitingModerationsUserFanFictionsById(string id);
        Task<List<FanFictionViewModel>> GetAwaitingPublicationsUserFanFictionsById(string id);
        Task<List<UserRating>> GetUserLikesById(string id);
        Task<List<FanFictionViewModel>> GetActiveFanFictions();
        Task<FandomViewModel> GetFandomById(int id);
        Task<List<FanFictionViewModel>> GetActiveFanFictionsByFilters(int? fandomId, string userName);
        Task<string> GetUserPhotoById(string id);
        Task<User> GetUserById(string id);
        Task AddComment(CommentViewModel model);
        Task<List<CommentViewModel>> GetUserCommentsActiveFanFictionsById(string id);
        Task<CommentViewModel> GetCommentById(int id);
        Task<string> RemoveCommentById(int id);
        Task<List<FanFictionViewModel>> GetActiveFanFictionsByName(string fanFictionName);
        Task SetLike(int id, string userId);
        Task RemoveLike(int id, string userId);
        Task<StatusViewModel> GetStatusById(int id);
        Task<int?> GetPreviousChapterById(int id);
        Task<int?> GetNextChapterById(int id);
        List<UserLikes> GetBestPostByUniqueFandom(int count);
        List<PopularFandom> GetBestFandoms(int count);
        List<FanFictionViewModel> GetLastAddedAndApproved(int count);
        Task<string> SetBlock(int fanFictionId, string userId);
    }
}