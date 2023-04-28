using courseproject_api.Dtos;
using courseproject_api.Models;

namespace courseproject_api.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(int id);
        bool UserExists(string email);
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string email);
        User AddUser(UserRequestDto user);
    }
}
