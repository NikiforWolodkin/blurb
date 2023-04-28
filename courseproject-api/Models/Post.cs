namespace courseproject_api.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public ICollection<User> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<User> Shares { get; set; }
    }
}
