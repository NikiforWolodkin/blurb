namespace courseproject_api.Models
{
    public class Subscription
    {
        public int SubscriberId { get; set; }
        public int PublisherId { get; set; }
        public User Subscriber { get; set; }
        public User Publisher { get; set; }
    }
}
