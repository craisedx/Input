using System;
using System.ComponentModel.DataAnnotations;

namespace Input.Models
{
    public class Moderation
    {
        public int Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
        
        [Required]
        public Status Status { get; set; }
        public int StatusId { get; set; }
        
        [MaxLength(600)]
        public string Message { get; set; }
        
        public DateTime ChangeTime { get; set; }
    }
}