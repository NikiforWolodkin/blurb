namespace courseproject_api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public User Author { get; set; }
    }
}
