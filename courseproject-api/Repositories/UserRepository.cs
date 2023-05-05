using courseproject_api.Data;
using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Models;
using Microsoft.EntityFrameworkCore;

namespace courseproject_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool UserExists(int id)
        {
            return _context.Users
                .Any(u => u.Id == id);
        }

        public bool UserExists(string email)
        {
            return _context.Users
                .Any(u => u.Email == email);
        }
        public ICollection<User> GetUsers()
        {
            return _context.Users
                .Include(u => u.Subscribers)
                .Include(u => u.Subscriptions)
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .Include(u => u.Likes)
                .Include(u => u.Shares)
                .Include(u => u.Reports)
                .ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users
                .Include(u => u.Subscribers)
                .Include(u => u.Subscriptions)
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .Include(u => u.Likes)
                .Include(u => u.Shares)
                .Include(u => u.Reports)
                .Where(u => u.Id == id)
                .FirstOrDefault();
        }

        public User GetUser(string email)
        {
            return _context.Users
                .Include(u => u.Subscribers)
                .Include(u => u.Subscriptions)
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .Include(u => u.Likes)
                .Include(u => u.Shares)
                .Include(u => u.Reports)
                .Where(u => u.Email == email)
                .FirstOrDefault();
        }

        public User AddUser(UserRequestDto userDto)
        {
            User user = new User();

            user.Email = userDto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.Username = userDto.Username;
            user.RegistrationDate = DateTime.UtcNow;
            user.Avatar = "Basic";
            user.ProfileColor = "Red";
            user.Status = "ACTIVE";
            user.Role = "USER";

            _context.Users.Add(user);

            _context.SaveChanges();

            return GetUser(user.Email);
        }

        public User UpdateUser(UserDto userDto)
        {
            User user = GetUser((int)userDto.Id);

            if (user.Username is not null)
                user.Username = userDto.Username;
            if (user.Avatar is not null)
                user.Avatar = userDto.Avatar;
            if (user.ProfileColor is not null)
                user.ProfileColor = userDto.ProfileColor;

            _context.SaveChanges();

            return user;
        }

        public User ChangeUserStatus(int id, string status)
        {
            User user = GetUser(id);

            user.Status = status;

            _context.SaveChanges();

            return user;
        }

        public ICollection<User> GetUserSubscriptions(int id)
        {
            return _context.Subscriptions
                .Include(s => s.Subscriber)
                .Include(s => s.Publisher)
                .Where(s => s.SubscriberId == id)
                .Select(s => s.Publisher)
                .ToList();
        }

        public bool AddUserSubscription(int subscriberId, int publisherId)
        {
            if (_context.Subscriptions.Any(s => s.SubscriberId == subscriberId 
                                           && s.PublisherId == publisherId))
            {
                return false;
            }

            Subscription subscription = new Subscription();

            subscription.SubscriberId = subscriberId;
            subscription.PublisherId = publisherId;

            _context.Subscriptions.Add(subscription);

            _context.SaveChanges();

            return true;
        }

        public bool RemoveUserSubscription(int subscriberId, int publisherId)
        {
            if (!_context.Subscriptions.Any(s => s.SubscriberId == subscriberId
                                               && s.PublisherId == publisherId))
            {
                return false;
            }

            Subscription subscription = _context.Subscriptions
                .Where(s => s.SubscriberId == subscriberId && s.PublisherId == publisherId)
                .FirstOrDefault();

            _context.Subscriptions.Remove(subscription);

            _context.SaveChanges();

            return true;
        }

        public bool DeleteUser(int id)
        {
            User user = GetUser(id);

            _context.Users.Remove(user);

            _context.SaveChanges();

            return true;
        }

        public bool IsUserSubscribed(int userId, int publisherId)
        {
            return _context.Subscriptions.Any(s => s.SubscriberId == userId && s.PublisherId == publisherId);
        }
    }
}
