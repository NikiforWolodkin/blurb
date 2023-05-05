using courseproject_api.Models;

namespace courseproject_api.Dtos
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Avatar { get; set; }
        public string? ProfileColor { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }
        public bool? IsSubscribed { get; set; }
        public bool? CanBan { get; set; }
    }
}
