using courseproject_api.Models;

namespace courseproject_api.Dtos
{
    public class PostDto
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorUsername { get; set; }
        public DateTime? CreationTime { get; set; }
        public int? LikeCount { get; set; }
        public int? CommentCount { get; set; }
        public int? ShareCount { get; set; }
        public int? ReportCount { get; set; }
    }
}
