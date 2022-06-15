using System.ComponentModel.DataAnnotations;

namespace Input.ViewModels.Status
{
    public class StatusViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
    }
}