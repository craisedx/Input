using System.ComponentModel.DataAnnotations;

namespace Input.Models
{
    public class FanFiction
    {
        public int Id { get; set; }
        
        public User User { get; set; }
        public string UserId { get; set; }
        
        [Required]
        [MaxLength(400)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string ShortDescription { get; set; }
        
        [Required]
        public Fandom Fandom { get; set; }
        public int FandomId { get; set; }
        
        [Required]
        public Moderation Moderation { get; set; }
        public int ModerationId { get; set; }
        
        public string Photo { get; set; }
    }
}