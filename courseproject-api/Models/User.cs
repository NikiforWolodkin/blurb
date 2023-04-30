using System.ComponentModel.DataAnnotations.Schema;

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
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Subscription> Subscribers { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Share> Shares { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Post> Posts { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
    }
}
