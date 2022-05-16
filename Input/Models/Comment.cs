using System;
using System.ComponentModel.DataAnnotations;

namespace Input.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        public FanFiction FanFiction { get; set; }
        public int FanFictionId { get; set; }
        
        public User User { get; set; }
        public string UserId { get; set; }
        
        [MaxLength(500)]
        public string CommentBody { get; set; }
        
        public DateTime SendingTime { get; set; }
    }
}