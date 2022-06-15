using System.ComponentModel.DataAnnotations;
using Input.Constants;
using Input.Constants.InfoMessages;

namespace Input.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = UserErrorsConstants.NameNotProvided)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = UserErrorsConstants.ErrorMessageMinLength)]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = UserErrorsConstants.AddressNotSpecified)]
        [EmailAddress(ErrorMessage = UserErrorsConstants.InvalidEmail)]
        public string Email { get; set; }

        [Required(ErrorMessage = UserErrorsConstants.PasswordNotSpecified)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = UserErrorsConstants.PasswordNotSpecified)]
        [Compare(UserInfoConstants.Password, ErrorMessage = UserErrorsConstants.PasswordCompareIncorrectly)]
        public string ConfirmPassword { get; set; }
    }
}