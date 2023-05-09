using courseproject_api.Models;

namespace courseproject_api.Dtos
{
    public class PostDto
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public ICollection<string>? Tags { get; set; }
        public int? AuthorId { get; set; }
        public bool? IsLiked { get; set; }
        public string? AuthorUsername { get; set; }
        public string? AuthorProfileColor { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? AuthorStatus { get; set; }
        public DateTime? CreationTime { get; set; }
        public int? LikeCount { get; set; }
        public int? CommentCount { get; set; }
        public int? ShareCount { get; set; }
        public int? ReportCount { get; set; }
    }
}
