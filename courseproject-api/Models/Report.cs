namespace courseproject_api.Models
{
    public class Report
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
