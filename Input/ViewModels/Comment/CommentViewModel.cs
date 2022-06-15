using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Input.Constants;
using Input.Constants.Display;
using Input.Models;
using Input.ViewModels.FanFiction;

namespace Input.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        
        public FanFictionViewModel FanFiction { get; set; }
        public int FanFictionId { get; set; }
        
        public Models.User User { get; set; }
        public string UserId { get; set; }
        
        [MaxLength(500, ErrorMessage = ErrorConstants.CommentBodyMaxLength)]
        [DisplayName(DisplayNameConstants.CommentBody)]
        [Required(ErrorMessage = ErrorConstants.CommentBodyRequired)]
        public string CommentBody { get; set; }
        
        public DateTime SendingTime { get; set; }
    }
}