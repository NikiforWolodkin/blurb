using courseproject_api.Models;

namespace courseproject_api.Dtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string ProfileColor { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<int> SubscriberIds { get; set; }
    }
}
