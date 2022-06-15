using Input.Models;
using Input.ViewModels.Comment;

namespace Input.ViewModels.FanFiction
{
    public class FanFictionPostViewModel
    {
        public FanFictionViewModel FanFiction{ get; set; }
        public CommentViewModel Comment { get; set; }
        public UserRating Like { get; set; }
    }
}