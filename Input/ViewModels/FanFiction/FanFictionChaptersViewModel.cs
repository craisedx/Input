using System.Collections.Generic;
using Input.ViewModels.Chapter;

namespace Input.ViewModels.FanFiction
{
    public class FanFictionChaptersViewModel
    {
        public FanFictionViewModel FanFiction{ get; set; }
        
        public List<ChapterViewModel> Chapters{ get; set; }
    }
}