using System.ComponentModel.DataAnnotations;

namespace Input.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(5000)]
        public string ChapterBody { get; set; }
        
        public FanFiction FanFiction { get; set; }
        public int FanFictionId { get; set; }
        
        public string Photo { get; set; }
    }
}