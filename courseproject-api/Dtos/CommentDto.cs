using courseproject_api.Models;

namespace courseproject_api.Dtos
{
    public class CommentDto
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public DateTime? CreationTime { get; set; }
        public int? PostId { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorUsername { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? AuthorProfileColor { get; set; }
        public string? AuthorStatus { get; set; }
    }
}
