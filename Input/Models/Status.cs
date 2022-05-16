using System.ComponentModel.DataAnnotations;

namespace Input.Models
{
    public class Status
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
    }
}