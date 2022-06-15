using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Input.Business.Interfaces;
using Input.Constants;
using Input.Constants.Admin;
using Input.Constants.InfoMessages;
using Input.Constants.Statuses;
using Input.Email;
using Input.Migrations;
using Input.Models;
using Input.ViewModels.Chapter;
using Input.ViewModels.Comment;
using Input.ViewModels.FanFiction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Input.Controllers
{
    public class FanFictionController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IFanFictionService fanFictionService;
        private readonly IUserService userService;
        private readonly IModerationService moderationService;
        private readonly IAdminService adminService;
        
        public FanFictionController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IFanFictionService fanFictionService,
            IUserService userService,
            IModerationService moderationService,
            IAdminService adminService)
        {
            this.moderationService = moderationService;
            this.adminService = adminService;
            this.fanFictionService = fanFictionService;
            this.userService = userService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public async Task RemoveLike(int id)
        {
            var user = User.Claims.ElementAt(0).Value;
            
            await fanFictionService.RemoveLike(id, user);
        }
        
        [HttpGet]
        [Authorize]
        public async Task SetLike(int id)
        {
            var user = User.Claims.ElementAt(0).Value;
            
            await fanFictionService.SetLike(id, user);
        }
        
        public async Task<IActionResult> FanFiction(int id, CommentViewModel comment = null)
        {
            var fanFiction = new FanFictionPostViewModel
            {
                FanFiction = await fanFictionService.GetFanFictionById(id),
                Comment = comment
            };

            if (fanFiction.FanFiction == null) return RedirectToAction("Index","Home");
            
            ViewBag.Chapters = await fanFictionService.GetChaptersByFanFictionId(fanFiction.FanFiction.Id);
            ViewBag.Comments = await fanFictionService.GetCommentsByFanFictionId(fanFiction.FanFiction.Id);
            
            return moderationService.CheckAccessToViewPost(User,fanFiction.FanFiction) ? 
                View(fanFiction) : RedirectToAction("Index","Home");
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Fandoms = await fanFictionService.GetAllFandom();
            
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddChapter(int id)
        { 
            var fanFiction = await fanFictionService.GetFanFictionById(id);
            
            if(fanFiction == null) return Redirect($"/ReadFanFiction/{id}");

            ViewBag.FanFictionId = fanFiction.Id;

            return View();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddChapter(ChapterViewModel chapter)
        {
            if (ModelState.IsValid)
            {
                var fanFictionId = await fanFictionService.AddChapter(chapter);
                
                return Redirect($"/ReadFanFiction/{fanFictionId}");
            }

            return View(chapter);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<string> RemoveChapter(int id)
        {
            var messageToUser = await fanFictionService.RemoveChapter(id);
            
            return messageToUser;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<string> RemoveFanFiction(int id)
        {
            var user = User.Claims.ElementAt(0).Value;
            var fanFiction = await fanFictionService.GetFanFictionById(id);
            var userIsAdmin = User.IsInRole(AdminConstants.AdminRole);
            
            if (fanFiction == null || fanFiction.UserId != user && !userIsAdmin) return ErrorConstants.ErrorRemoveFanFiction;
            
            var messageToUser = await fanFictionService.RemoveFanFiction(id);
            
            return messageToUser;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<string> RemoveComment(int id)
        {
            var user = User.Claims.ElementAt(0).Value;
            var comment = await fanFictionService.GetCommentById(id);
            var userIsAdmin = User.IsInRole(AdminConstants.AdminRole);
            
            if (comment == null || comment.UserId != user && !userIsAdmin) return ErrorConstants.ErrorRemoveComment;
            
            var messageToUser = await fanFictionService.RemoveCommentById(id);
            
            return messageToUser;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditChapter(int id)
        {
            var chapter = await fanFictionService.GetChapterById(id);

            if(chapter == null) return Redirect($"/ReadFanFiction/{id}");
            
            ViewBag.FanFictionId = chapter.FanFictionId;
            
            return View(chapter);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditChapter(ChapterViewModel model)
        {
            var user = User.Claims.ElementAt(0).Value;
            var fanFiction = await fanFictionService.GetFanFictionById(model.FanFictionId);
            
            if (fanFiction == null || fanFiction.UserId != user) return RedirectToAction("Index", "Home");
                
            if (ModelState.IsValid)
            {
                var addedChapterFanFictionId = await fanFictionService.EditChapter(model);
                
                return Redirect($"/ReadFanFiction/{addedChapterFanFictionId}");
            }

            return View(model);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditFanFiction(int id)
        {
            ViewBag.Fandoms = await fanFictionService.GetAllFandom();
            var fanFiction = await fanFictionService.GetFanFictionById(id);
            
            if(fanFiction == null) return Redirect($"/ReadFanFiction/{id}");

            if (fanFiction.Moderation != null && fanFiction.Moderation.Status.Name == StatusesConstants.RejectedStatus)
                await moderationService.SetStatusAwaitProcessed(fanFiction.Moderation.Id);
            
            return View(fanFiction);
        }

        public IActionResult RemoveFilter(int filterId, string userName, int? fandomId)
        {
            if (!fandomId.HasValue && string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("AllFanFictions", "FanFiction");
            }
            
            switch (filterId)
            {
                case 1:
                    return RedirectToAction("AllFanFictions","FanFiction", new { userName });
                case 2:
                    return RedirectToAction("AllFanFictions","FanFiction", new { fandomId });
            }

            return RedirectToAction("AllFanFictions", "FanFiction");
        }
        
        public IActionResult AddFilter(int? fandomId, string userName)
        {
            if (!fandomId.HasValue && string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("AllFanFictions", "FanFiction");
            }
            
            return RedirectToAction("AllFanFictions","FanFiction", new { fandomId, userName });
        }

        public async Task<IActionResult> AllFanFictions(int? fandomId, string userName)
        {
            if (userName == "undefined") userName = null;
            
            List<FanFictionViewModel> fanFictions;
            
            if (!fandomId.HasValue && string.IsNullOrEmpty(userName))
            {
                fanFictions = await fanFictionService.GetActiveFanFictions();
            }
            else
            {
                fanFictions = await fanFictionService.GetActiveFanFictionsByFilters(fandomId, userName);
            }
            
            if (fandomId.HasValue)
            {
                var fandom = await fanFictionService.GetFandomById(fandomId.Value);
                
                ViewBag.Fandom = fandom;
            }
            else
            {
                ViewBag.Fandom = null;
            }

            ViewBag.Fandoms = await fanFictionService.GetAllFandom();

            ViewBag.UserName = userName;
            
            return View(fanFictions);
        }

        [Route("ReadFanFiction/{fanFictionId:int}/{chapterId:int?}")]
        public async Task<IActionResult> GetFanFictionChapterById(int fanFictionId, int? chapterId, CommentViewModel comment = null)
        {
            var fanFiction = new FanFictionPostViewModel
            {
                FanFiction = await fanFictionService.GetFanFictionById(fanFictionId),
                Comment = comment
            }; 
            
            if (fanFiction.FanFiction == null) return RedirectToAction("Index","Home");

            ViewBag.Comments = await fanFictionService.GetCommentsByFanFictionId(fanFiction.FanFiction.Id);
            
            if (!chapterId.HasValue)
            {
                ViewBag.Chapters = await fanFictionService.GetChaptersByFanFictionId(fanFiction.FanFiction.Id);
                
                return moderationService.CheckAccessToViewPost(User,fanFiction.FanFiction) ? 
                    View("FanFictionChapterList",fanFiction) : RedirectToAction("Index","Home");
            }

            ViewBag.Chapter = await fanFictionService.GetChapterById(chapterId.Value);
            
            var previousChapter = await fanFictionService.GetPreviousChapterById(chapterId.Value);
            
            var nextChapter = await fanFictionService.GetNextChapterById(chapterId.Value);

            if (previousChapter.HasValue) ViewBag.PreviousChapter = previousChapter.Value;
            if (nextChapter.HasValue) ViewBag.NextChapter = nextChapter.Value;

            return moderationService.CheckAccessToViewPost(User,fanFiction.FanFiction) ? 
                View("FanFictionChapterId",fanFiction) : RedirectToAction("Index","Home");
        }
        
        public async Task<IActionResult> Search(string fanFictionName)
        {
            var admins = await adminService.GetAllAdmins();
            var fanFictions = await fanFictionService.GetActiveFanFictionsByName(fanFictionName);
            ViewBag.FanFictionName = fanFictionName;
            
            return View(fanFictions);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel comment,int? chapterId)
        {
            comment.UserId = User.Claims.ElementAt(0).Value;

            if (ModelState.IsValid)
            {
                await fanFictionService.AddComment(comment);
                
                return RedirectToAction("GetFanFictionChapterById", new { fanFictionId=comment.FanFictionId, chapterId });
            }
            
            if (chapterId.HasValue)
                return Redirect($"/ReadFanFiction/{comment.FanFictionId}/{chapterId.Value}/?comment={comment}");
            
            return Redirect($"/ReadFanFiction/{comment.FanFictionId}/?comment={comment}");
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditFanFiction(FanFictionViewModel model)
        {
            var user = User.Claims.ElementAt(0).Value;
            var userIsAdmin = User.IsInRole(AdminConstants.AdminRole);

            if (model.UserId != user)
            {
                if (!userIsAdmin) return RedirectToAction("Index", "Home");
            }
            
            if (ModelState.IsValid)
            {
                var addedFanFictionId = await fanFictionService.EditFanFiction(model);
                
                return Redirect($"/ReadFanFiction/{addedFanFictionId}");
            }

            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(FanFictionViewModel model)
        {
            var user = User.Claims.ElementAt(0).Value;
            model.User = await userService.GetUserById(user);
            
            if (model.User == null) return RedirectToAction("Index", "Home");
            
            if (ModelState.IsValid)
            {
                var addedFanFictionId = await fanFictionService.CreateFanFiction(model);
                
                return Redirect($"/ReadFanFiction/{addedFanFictionId}");
            }

            return View(model);
        }
        
        public async Task SendAllAdminMessage()
        {
            var callbackUrl = Url.Action(
                "AllPosts",
                "Moderation",
                new {},
                protocol: HttpContext.Request.Scheme);

            var allAdmins = await adminService.GetAllAdmins();

            foreach (var item in allAdmins)
            {
                await EmailWork.SendEmailDefault(item.Email, UserInfoConstants.InfoMessage,
                    UserInfoConstants.SendAdminToNewPublicationEmail(item.UserName,callbackUrl));
            }
        }

        [Authorize]
        public async Task<string> PublicationFanFiction(int id)
        {
            var userId = User.Claims.ElementAt(0).Value;
            var fanFiction = await fanFictionService.GetFanFictionById(id);

            if (string.IsNullOrEmpty(userId) || fanFiction.UserId != userId ) return ErrorConstants.ErrorPublicationFanFiction;
            
            var messageToUser = await fanFictionService.PublicationFanFiction(id);
            
            if (messageToUser == InfoConstants.SuccessSendPublicationFanFiction)
            {
                await SendAllAdminMessage();
            }
                
            return messageToUser;
        }
    }
}