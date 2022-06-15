using System.Linq;
using System.Threading.Tasks;
using Input.Business.Interfaces;
using Input.Constants;
using Input.Constants.Admin;
using Input.Constants.InfoMessages;
using Input.Constants.Statuses;
using Input.Email;
using Input.Models;
using Input.ViewModels.FanFiction;
using Input.ViewModels.Moderation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Input.Controllers
{
    public class ModerationController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IFanFictionService fanFictionService;
        private readonly IUserService userService;
        private readonly IModerationService moderationService;
        
        public ModerationController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IFanFictionService fanFictionService,
            IUserService userService,
            IModerationService moderationService)
        {
            this.moderationService = moderationService;
            this.fanFictionService = fanFictionService;
            this.userService = userService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize (Roles=AdminConstants.AdminRole)]
        public async Task<IActionResult> Index()
        {
            var awaitingModerationFanFiction = await moderationService.GetAwaitingModerationFanFiction();
            var adminModerationFanFiction = await moderationService.GetActiveAdminModerationFanFiction(User);
            ViewBag.AdminModeration = adminModerationFanFiction;
            
            return View(awaitingModerationFanFiction);
        }

        [Route("/Posts/Awaiting")]
        [Authorize (Roles=AdminConstants.AdminRole)]
        public async Task<IActionResult> FreePosts()
        {
            var awaitingModerationFanFiction = await moderationService.GetAwaitingModerationFanFiction();

            return View(awaitingModerationFanFiction);
        }
        
        [Route("/Posts/AdminPost")]
        [Authorize (Roles=AdminConstants.AdminRole)]
        public async Task<IActionResult> AdminPosts(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var adminModerationFanFiction = await moderationService.GetActiveAdminModerationFanFiction(User);
                
                return View(adminModerationFanFiction);
            }

            var user = await fanFictionService.GetUserById(id);
            if (user == null) return RedirectToAction("Index", "Home");
            
            var adminModerationFanFictionById = await moderationService.GetActiveAdminModerationFanFiction(User);
                
            return View(adminModerationFanFictionById);
        }
        
        [Route("/Posts/All")]
        [Authorize (Roles=AdminConstants.AdminRole)]
        public async Task<IActionResult> AllPosts()
        {
            var allFanFiction = await moderationService.GetAllFanFictions();

            return View(allFanFiction);
        }
        
        [Authorize(Roles = AdminConstants.AdminRole)]
        public async Task<IActionResult> CheckPost(int id)
        {
            var fanFiction = new FanFictionChaptersViewModel()
            {
                FanFiction = await fanFictionService.GetFanFictionById(id),
                Chapters = await fanFictionService.GetChaptersByFanFictionId(id)
            };

            if (fanFiction.FanFiction == null) return RedirectToAction("Index", "Moderation");

            ViewBag.ModerationStatuses = await moderationService.GetAllStatuses();
            
            return View(fanFiction);
        }

        [HttpPost]
        [Authorize(Roles = AdminConstants.AdminRole)]
        public async Task<IActionResult> CheckedFanFiction(ModerationViewModel moderation)
        {
            var status = await fanFictionService.GetStatusById(moderation.StatusId);
            
            if (status.Name == StatusesConstants.AwaitProcessingStatus)
                await moderationService.RemoveAdminToModeration(moderation.Id);
                
            await moderationService.CheckedFanFiction(moderation);
            
            var fanFictionId = await moderationService.GetFanFictionIdByModerationId(moderation.Id);
            var fanFiction = await fanFictionService.GetFanFictionById(fanFictionId);

            if (fanFiction.Moderation.Status.Name == StatusesConstants.ApprovedStatus)
            {
                await SendUserApprovePublicationEmail(fanFiction);
            }
            else
            {
                await SendUserToUpdatePublicationEmail(fanFiction);
            }

            return RedirectToAction("FreePosts", "Moderation");
        }

        public async Task SendUserToAddAdminToPublicationMessage(FanFictionViewModel fanFiction)
        {
            var callbackUrl = Url.Action(
                "AllPosts",
                "Moderation",
                new {},
                protocol: HttpContext.Request.Scheme);

            await EmailWork.SendEmailDefault(fanFiction.User.Email, UserInfoConstants.InfoMessage,
                UserInfoConstants.SendUserToAddAdminToPublicationEmail(fanFiction,callbackUrl));
        }
        
        public async Task SendUserToUpdatePublicationEmail(FanFictionViewModel fanFiction)
        {
            var callbackUrl = Url.Action(
                "AllPosts",
                "Moderation",
                new {},
                protocol: HttpContext.Request.Scheme);

            await EmailWork.SendEmailDefault(fanFiction.User.Email, UserInfoConstants.InfoMessage,
                UserInfoConstants.SendUserToUpdatePublicationEmail(fanFiction,callbackUrl));
        }
        
        public async Task SendUserApprovePublicationEmail(FanFictionViewModel fanFiction)
        {
            var callbackUrl = Url.Action(
                "AllPosts",
                "Moderation",
                new {},
                protocol: HttpContext.Request.Scheme);

            await EmailWork.SendEmailDefault(fanFiction.User.Email, UserInfoConstants.InfoMessage,
                UserInfoConstants.SendUserApprovePublicationEmail(fanFiction,callbackUrl));
        }
        
        [Authorize(Roles = AdminConstants.AdminRole)]
        public async Task<string> SetAdminChecked(int id)
        {
            var userId = User.Claims.ElementAt(0).Value;
            var userIsAdmin = User.IsInRole(AdminConstants.AdminRole);

            if (string.IsNullOrEmpty(userId) || !userIsAdmin) return ErrorConstants.ErrorAddAdminToFanFictionModeration;
            
            var messageToUser = await moderationService.SetAdminToModeration(id, userId);

            if (messageToUser != InfoConstants.SuccessAddAdminToFanFictionModeration) return messageToUser;
            
            var fanFiction = await fanFictionService.GetFanFictionById(id);
                
            await SendUserToAddAdminToPublicationMessage(fanFiction);

            return messageToUser;
        }
    }
}