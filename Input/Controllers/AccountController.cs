using System.Threading.Tasks;
using Input.Constants;
using Input.Constants.InfoMessages;
using Input.Email;
using Input.Migrations;
using Input.Models;
using Input.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Input.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext db;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        
        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationContext context)
        {
            db = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByNameAsync(model.UserName).Result;
                var signInResult = await signInManager
                    .PasswordSignInAsync(model.UserName, model.Password, true, false);
                if (signInResult.Succeeded)
                {
                    if (user.EmailConfirmed)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                            return Redirect(model.ReturnUrl);

                        await db.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, UserErrorsConstants.EmailNotConfirmed);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, UserErrorsConstants.LoginPasswordEnteredIncorrectly);
                }
            }

            return View(model);
        }

        [Route("Confirm")]
        public IActionResult Confirm()
        {
            return View();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                return View("Error");
            }

            var result = await userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded) return View("Error");
            
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email, UserName = model.UserName,
                    UserPhoto = UserInfoConstants.BaseUserPhoto
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new {userId = user.Id, code = code},
                        protocol: HttpContext.Request.Scheme);

                    await EmailWork.SendEmailDefault(model.Email, UserInfoConstants.AccountConfirmation,
                        UserInfoConstants.SendConfirmEmail(callbackUrl));
                    
                    return RedirectToAction("Confirm", "Account");
                }

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}