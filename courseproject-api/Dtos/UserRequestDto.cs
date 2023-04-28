using System.ComponentModel.DataAnnotations;

namespace courseproject_api.Dtos
{
    public class UserRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Username { get; set; }
    }
}
