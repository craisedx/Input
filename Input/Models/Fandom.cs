using System.ComponentModel.DataAnnotations;

namespace Input.Models
{
    public class Fandom
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}