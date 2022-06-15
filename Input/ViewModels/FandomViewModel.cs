using System.ComponentModel.DataAnnotations;

namespace Input.ViewModels
{
    public class FandomViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}