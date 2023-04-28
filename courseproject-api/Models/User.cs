namespace courseproject_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string ProfileColor { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<User> Subscribers { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
