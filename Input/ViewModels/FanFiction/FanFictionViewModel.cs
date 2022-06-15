using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Input.Constants;
using Input.Constants.Display;
using Input.Models;
using Input.ViewModels.Moderation;

namespace Input.ViewModels.FanFiction
{
    public class FanFictionViewModel
    {
        public int Id { get; set; }
        
        public Models.User User { get; set; }
        public string UserId { get; set; }
        
        [DisplayName(DisplayNameConstants.NameFanFiction)]
        [Required(ErrorMessage = ErrorConstants.NameRequired)]
        [MaxLength(400, ErrorMessage = ErrorConstants.NameMaxLength)]
        public string Name { get; set; }
        
        [DisplayName(DisplayNameConstants.ShortDescription)]
        [Required(ErrorMessage = ErrorConstants.ShortDescriptionRequired)]
        [MaxLength(1000, ErrorMessage = ErrorConstants.ShortDescriptionMaxLength)]
        public string ShortDescription { get; set; }
        
        public Fandom Fandom { get; set; }
        
        [DisplayName(DisplayNameConstants.Fandom)]
        [Required(ErrorMessage = ErrorConstants.FandomRequired)]
        public int FandomId { get; set; }
        
        public ModerationViewModel Moderation { get; set; }
        public int? ModerationId { get; set; }
        
        [DisplayName(DisplayNameConstants.ImageFanFiction)]
        public string Photo { get; set; }
        
        public List<UserRating> UserRating { get; set; }
    }
}