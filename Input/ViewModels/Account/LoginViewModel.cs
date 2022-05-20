using System.ComponentModel.DataAnnotations;
using Input.Constants;

namespace Input.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = UserErrorsConstants.NameNotProvided)]
        public string UserName { get; set; }

        [Required(ErrorMessage = UserErrorsConstants.PasswordNotSpecified)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}