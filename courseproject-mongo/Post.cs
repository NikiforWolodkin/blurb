using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace courseproject_mongo
{
    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public ObjectId AuthorId { get; set; }
        public List<ObjectId> Likes { get; set; }
        public List<ObjectId> Shares { get; set; }
        public List<ObjectId> Reports { get; set; }
        public List<Comment> Comments { get; set; }

        public class Comment
        {
            public ObjectId AuthorId { get; set; }
            public string Text { get; set; }
            public DateTime CreationTime { get; set; }
        }
    }
}
