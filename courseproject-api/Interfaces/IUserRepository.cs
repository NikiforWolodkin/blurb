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
        ICollection<User> GetUserSubscriptions(int id);
        bool AddUserSubscription(int subscriberId, int publisherId);
        bool RemoveUserSubscription(int subscriberId, int publisherId);

        User AddUser(UserRequestDto user);
        User UpdateUser(UserDto user);
        bool DeleteUser(int id);
        User ChangeUserStatus(int id, string status);
    }
}
