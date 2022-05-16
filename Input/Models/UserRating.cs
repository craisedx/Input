namespace Input.Models
{
    public class UserRating
    {
        public int Id { get; set; }
        
        public FanFiction FanFiction { get; set; }
        public int FanFictionId { get; set; }
        
        public User User { get; set; }
        public string UserId { get; set; }
    }
}