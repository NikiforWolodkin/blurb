using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace courseproject_mongo
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string ProfileColor { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public List<ObjectId> Subscriptions { get; set; }
    }
}
