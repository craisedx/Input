using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Input.Constants;
using Input.Constants.Display;

namespace Input.ViewModels.Account
{
    public class EditProfileViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [DisplayName(DisplayNameConstants.Photo)]
        [Required(ErrorMessage = ErrorConstants.PhotoRequired)]
        public string Photo { get; set; }
    }
}