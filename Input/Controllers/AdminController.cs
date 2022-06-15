using System.Linq;
using System.Threading.Tasks;
using Input.Business.Interfaces;
using Input.Constants.Admin;
using Input.Constants.InfoMessages;
using Input.Email;
using Input.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Input.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminService adminService;
        private readonly IFanFictionService fanFictionService;
        
        public AdminController(IAdminService adminService,
            IFanFictionService fanFictionService)
        {
            this.adminService = adminService;
            this.fanFictionService = fanFictionService;
        }

        [Authorize (Roles=AdminConstants.AdminRole)]
        public IActionResult AdminStatus()
        {
            return View();
        }
        
        [Authorize (Roles=AdminConstants.AdminRole)]
        public IActionResult AdminFandom()
        {
            return View();
        }
        
        public JsonResult GetAllStatuses()
        {
            var statuses = adminService.GetAllStatuses();

            return Json(statuses);
        }
        
        public JsonResult GetAllFandoms()
        {
            var statuses = adminService.GetAllFandoms();

            return Json(statuses);
        }

        [HttpPost]
        [Authorize (Roles=AdminConstants.AdminRole)]
        public IActionResult UpdateFandom(FandomViewModel model)
        {
            var fandom = adminService.UpdateFandom(model);

            return Json(fandom);
        }
        
        [HttpPost]
        [Authorize (Roles=AdminConstants.AdminRole)]
        public IActionResult CreateFandom(FandomViewModel model)
        {
            var fandom = adminService.CreateFandom(model);

            return Json(fandom);
        }

        [Authorize (Roles=AdminConstants.AdminRole)]
        public async Task<IActionResult> AdminUser()
        {
            var users = await adminService.GetAllUserWithRoles();

            ViewBag.CountUsers = await adminService.GetCountUsers();
            ViewBag.CountBlocked = await adminService.GetCountBlockedUsers();
            ViewBag.CountAdmins = await adminService.GetCountUserAdmins();
            
            return View(users);
        }

        [Authorize (Roles=AdminConstants.AdminRole)]
        public async Task<IActionResult> BlockUser(string id)
        {
            await adminService.BlockUser(id);

            return RedirectToAction("AdminUser", "Admin");
        }

        [Authorize (Roles=AdminConstants.AdminRole)]
        public async Task<string> Block(int id)
        {
            var userId = User.Claims.ElementAt(0).Value;
            if (string.IsNullOrEmpty(userId)) return InfoConstants.RejectBlockFanFiction;

            var messageToUser = await fanFictionService.SetBlock(id, userId);

           return messageToUser;
        }
        
        public async Task<IActionResult> UnBlockUser(string id)
        {
            await adminService.UnBlockUser(id);

            return RedirectToAction("AdminUser", "Admin");
        }
        
        public async Task<IActionResult> AddAdmin(string id)
        {
            await adminService.AddAdmin(id);

            return RedirectToAction("AdminUser", "Admin");
        }
        
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            await adminService.DeleteAdmin(id);

            return RedirectToAction("AdminUser", "Admin");
        }
    }
}