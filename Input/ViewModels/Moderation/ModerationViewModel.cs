using System;
using System.ComponentModel.DataAnnotations;
using Input.Models;
using Input.ViewModels.Status;

namespace Input.ViewModels.Moderation
{
    public class ModerationViewModel
    {
        public int Id { get; set; }

        public Models.User User { get; set; }
        public string UserId { get; set; }
        
        [Required]
        public StatusViewModel Status { get; set; }
        public int StatusId { get; set; }
        
        [MaxLength(600)]
        public string Message { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}