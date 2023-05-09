using courseproject_api.Data;
using courseproject_api.Dtos;
using courseproject_api.Helper;
using courseproject_api.Interfaces;
using courseproject_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;

namespace courseproject_api.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public PostRepository(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public bool DeleteReports(int id)
        {
            Report[] reports = _context.Reports
                .Where(r => r.PostId == id)
                .ToArray();

            _context.Reports.RemoveRange(reports);

            _context.SaveChanges();

            return true;
        }

        public ICollection<KeyValuePair<User, Post>> GetPosts()
        {
            IList<User> users = _userRepository.GetUsers().ToList();

            IList<KeyValuePair<User, Post>> posts = new List<KeyValuePair<User, Post>>();

            foreach (var user in users)
            {
                foreach (var post in user.Posts)
                {
                    posts.Add(new KeyValuePair<User, Post>(user, post));
                }
            }

            return posts;
        }

        public ICollection<KeyValuePair<User, Post>> GetReportedPosts()
        {
            return GetPosts()
                .Where(k => k.Value.Reports.Count() > 0)
                .ToList();
        }

        public ICollection<KeyValuePair<User, Post>> GetPosts(int userId)
        {
            IList<User> users = _userRepository.GetUsers()
                .Where(u => u.Id == userId)
                .ToList();

            IList<KeyValuePair<User, Post>> posts = new List<KeyValuePair<User, Post>>();

            foreach (var user in users)
            {
                foreach (var post in user.Posts)
                {
                    posts.Add(new KeyValuePair<User, Post>(user, post));
                }
            }

            return posts;
        }

        public ICollection<KeyValuePair<User, Post>> GetTrendingPosts(int page)
        {
            return GetPosts()
                .Where(k => k.Value.CreationTime > DateTime.UtcNow.AddDays(-3))
                // .OrderByDescending(k => k.Value.Likes)
                .Skip(20 * page)
                .Take(20)
                .ToList();
        }

        public ICollection<KeyValuePair<User, Post>> GetSubscriptionPosts(int page, int userId)
        {
            ICollection<int> subscriptions = _userRepository.GetUserSubscriptions(userId)
                .Select(u => u.Id)
                .ToList();

            return GetPosts()
                .Where(k => subscriptions.Contains(k.Key.Id))
                // .OrderByDescending(k => k.Value.CreationTime)
                .Skip(20 * page)
                .Take(20)
                .ToList();
        }

        public KeyValuePair<User, Post> GetPost(int id)
        {
            User user = _userRepository.GetUsers()
                .Where(u => u.Posts.Any(p => p.Id == id))
                .FirstOrDefault();

            Post post = user.Posts
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return new KeyValuePair<User, Post>(user, post);
        }

        public bool PostExists(int id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        public KeyValuePair<User, Post> AddPost(PostDto postDto)
        {
            Post post = new Post();

            post.Text = HashtagFinder.RemoveHashtags(postDto.Text);
            post.Tags = HashtagFinder.FindHashtags(postDto.Text).Select(t => new Tag() { Text = t }).ToList();
            post.CreationTime = DateTime.UtcNow;

            User user = _userRepository.GetUser((int)postDto.AuthorId);

            user.Posts.Add(post);

            _context.SaveChanges();

            post = GetPost(post.Id).Value;

            return new KeyValuePair<User, Post>(user, post);
        }

        public int GetPostLikeCount(int id)
        {
            return _context.Posts
                .Include(p => p.Likes)
                .Where (p => p.Id == id)
                .FirstOrDefault()
                .Likes
                .Count();
        }

        public int GetPostShareCount(int id)
        {
            return _context.Posts
                   .Include(p => p.Shares)
                   .Where(p => p.Id == id)
                   .FirstOrDefault()
                   .Shares
                   .Count();
        }

        public int GetPostCommentCount(int id)
        {
            return _context.Posts
                .Include(p => p.Comments)
                .Where(p => p.Id == id)
                .FirstOrDefault()
                .Comments
                .Count();
        }

        public int GetPostReportCount(int id)
        {
            return _context.Posts
                   .Include(p => p.Reports)
                   .Where(p => p.Id == id)
                   .FirstOrDefault()
                   .Reports
                   .Count();
        }

        public ICollection<Comment> GetPostComments(int postId)
        {
            return _userRepository.GetUsers()
                .Select(u => u.Comments)
                .SelectMany(l => l) 
                .Where(c => c.PostId == postId)
                .ToList();
        }

        public bool AddLikeToPost(int postId, int userId)
        {
            if (_context.Likes.Any(l => l.PostId == postId && l.UserId == userId))
            {
                return false;
            }

            Like like = new Like();

            like.UserId = userId;
            like.PostId = postId;

            _context.Likes.Add(like);

            _context.SaveChanges();

            return true;
        }

        public bool RemoveLikeFromPost(int postId, int userId)
        {
            if (!_context.Likes.Any(l => l.PostId == postId && l.UserId == userId))
            {
                return false;
            }

            Like like = _context.Likes
                .Where(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefault();

            _context.Likes.Remove(like);

            _context.SaveChanges();

            return true;
        }

        public Comment AddCommentToPost(int postId, CommentDto commentDto)
        {
            Comment comment = new Comment();

            comment.PostId = postId;
            comment.UserId = (int)commentDto.AuthorId;
            comment.Text = commentDto.Text;
            comment.CreationTime = DateTime.UtcNow;
            
            _context.Comments.Add(comment);

            _context.SaveChanges();

            return _context.Comments
                .Include(c => c.User)
                .Where(c => c.Id == comment.Id)
                .FirstOrDefault();
        }

        public bool AddShareToPost(int postId, int userId)
        {
            if (_context.Shares.Any(l => l.PostId == postId && l.UserId == userId))
            {
                return false;
            }

            Share share = new Share();

            share.UserId = userId;
            share.PostId = postId;

            _context.Shares.Add(share);

            _context.SaveChanges();

            return true;
        }

        public bool AddReportToPost(int postId, int userId)
        {
            if (_context.Reports.Any(l => l.PostId == postId && l.UserId == userId))
            {
                return false;
            }

            Report report = new Report();

            report.UserId = userId;
            report.PostId = postId;

            _context.Reports.Add(report);

            _context.SaveChanges();

            return true;
        }

        public bool DeletePost(int id)
        {
            Post post = GetPost(id).Value;

            _context.Posts.Remove(post);

            _context.SaveChanges();

            return true;
        }

        public bool IsPostLiked(int postId, int userId)
        {
            return _context.Likes.Any(l => l.PostId == postId && l.UserId == userId);
        }

        public StatsDto GetStats()
        {
            StatsDto stats = new StatsDto();

            stats.Users = _context.Users.Count();
            stats.Posts = _context.Posts.Count();
            stats.Likes = _context.Likes.Count();
            stats.Shares = _context.Shares.Count();
            stats.Comments = _context.Comments.Count();
            stats.Reports = _context.Reports.Count();
            stats.BlockedUsers = _context.Users.Where(u => u.Status == "BANNED").Count();

            return stats;
        }
    }
}
