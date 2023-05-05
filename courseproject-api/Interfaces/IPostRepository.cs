using courseproject_api.Dtos;
using courseproject_api.Models;

namespace courseproject_api.Interfaces
{
    public interface IPostRepository
    {
        public StatsDto GetStats();
        public ICollection<KeyValuePair<User, Post>> GetPosts();
        public ICollection<KeyValuePair<User, Post>> GetPosts(int userId);
        public ICollection<KeyValuePair<User, Post>> GetTrendingPosts(int page);
        public ICollection<KeyValuePair<User, Post>> GetSubscriptionPosts(int page, int userId);
        public KeyValuePair<User, Post> GetPost(int id);
        public bool PostExists(int id);
        public KeyValuePair<User, Post> AddPost(PostDto post);
        public int GetPostLikeCount(int Id);
        public int GetPostShareCount(int Id);
        public int GetPostCommentCount(int Id);
        public int GetPostReportCount(int Id);
        public ICollection<Comment> GetPostComments(int id);
        public bool AddLikeToPost(int postId, int  userId);
        public bool RemoveLikeFromPost(int postId, int userId);
        public Comment AddCommentToPost(int postId, CommentDto comment);
        public bool AddShareToPost(int postId, int userId);
        public bool AddReportToPost(int postId, int userId);
        public bool DeletePost(int id);
        public bool IsPostLiked(int postId, int userId);
    }
}
