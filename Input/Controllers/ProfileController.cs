using System.Linq;
using System.Threading.Tasks;
using Input.Business.Interfaces;
using Input.Constants.Admin;
using Input.Models;
using Input.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Input.Controllers
{
    public class ProfileController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IFanFictionService fanFictionService;
        private readonly IUserService userService;
        private readonly IModerationService moderationService;
        
        public ProfileController(
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

        public async Task<string> GetUserImage(string id)
        {
            return await fanFictionService.GetUserPhotoById(id);
        }

        public IActionResult GetUserPage(string id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole(AdminConstants.AdminRole))
            {
                ViewData["User"] = id;
                
                return View();
            }

            if (User.Identity.IsAuthenticated && User.Claims.ElementAt(0).Value == id)
            {
                return RedirectToAction("MyProfile", "Profile");
            }

            return RedirectToAction("UserProfile", "Profile",  new {id});
        }

        public async Task<IActionResult> UserProfile(string id)
        {
            var user = await fanFictionService.GetUserById(id);

            ViewBag.UserFanFictions = await fanFictionService.GetActiveUserFanFictionsById(id);
            
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.Claims.ElementAt(0).Value;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Home");

            var editProfile = await userService.GetEditUserById(userId);

            return View(editProfile);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = await userService.EditProfile(model);
                
                return userId == null ? RedirectToAction("Index", "Home") 
                    : RedirectToAction("MyProfile", "Profile");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyProfile() =>
           await MyProfileByMe();
        
        [HttpGet]
        [Authorize(Roles = AdminConstants.AdminRole)]
        private async Task<IActionResult> MyProfileByOther(string id)
        {
            var isAdmin = User.IsInRole(AdminConstants.AdminRole);
            
            if (!isAdmin) return Redirect("/Profile/MyProfile");
            
            var user = await fanFictionService.GetUserById(id);
            
            ViewBag.UserFanFictions = await fanFictionService.GetActiveUserFanFictionsById(id);
            
            ViewBag.AwaitingModerationsFanFictions = await fanFictionService.GetAwaitingModerationsUserFanFictionsById(id);
            
            ViewBag.AwaitingPublicationsFanFictions = await fanFictionService.GetAwaitingPublicationsUserFanFictionsById(id);
            
            ViewBag.UserLikes = await fanFictionService.GetUserLikesById(id);
            
            ViewBag.Comments = await fanFictionService.GetUserCommentsActiveFanFictionsById(id);

            ViewBag.IsAdmin = await userService.UserIsAdminById(id);
            
            return View(user);
        }
        
        [HttpGet]
        [Authorize]
        private async Task<IActionResult> MyProfileByMe()
        {
            var userId = User.Claims.ElementAt(0).Value;
            var user = await fanFictionService.GetUserById(userId);
            
            ViewBag.UserFanFictions = await fanFictionService.GetActiveUserFanFictionsById(userId);
            
            ViewBag.AwaitingModerationsFanFictions = await fanFictionService.GetAwaitingModerationsUserFanFictionsById(userId);
            
            ViewBag.AwaitingPublicationsFanFictions = await fanFictionService.GetAwaitingPublicationsUserFanFictionsById(userId);
            
            ViewBag.UserLikes = await fanFictionService.GetUserLikesById(userId);

            ViewBag.Comments = await fanFictionService.GetUserCommentsActiveFanFictionsById(userId);
            
            ViewBag.IsAdmin = await userService.UserIsAdminById(userId);

            return View(user);
        }
    }
}