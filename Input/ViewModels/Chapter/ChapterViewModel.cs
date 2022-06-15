using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Input.Constants;
using Input.Constants.Display;

namespace Input.ViewModels.Chapter
{
    public class ChapterViewModel
    {
        public int Id { get; set; }
        
        [DisplayName(DisplayNameConstants.NameChapter)]
        [Required(ErrorMessage = ErrorConstants.NameRequired)]
        [MaxLength(200, ErrorMessage = ErrorConstants.NameChapterMaxLength)]
        public string Name { get; set; }
        
        [DisplayName(DisplayNameConstants.ChapterBody)]
        [Required(ErrorMessage = ErrorConstants.ChapterBodyRequired)]
        public string ChapterBody { get; set; }
        
        public Models.FanFiction FanFiction { get; set; }
        public int FanFictionId { get; set; }
        
        [DisplayName(DisplayNameConstants.ImageFanFiction)]
        public string Photo { get; set; }
    }
}