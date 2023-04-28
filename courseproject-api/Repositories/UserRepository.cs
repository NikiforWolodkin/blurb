using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Models;

namespace courseproject_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        public static List<User> Users { get; set; } = new List<User>();

        public bool UserExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(string email)
        {
            return Users.Where(u => u.Email == email).Any();
        }
        public ICollection<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string email)
        {
            return Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public User AddUser(UserRequestDto user)
        {
            User newUser = new User();

            newUser.Email = user.Email;
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            newUser.Username = user.Username;

            Users.Add(newUser);

            return newUser;
        }
    }
}
