namespace courseproject_api.Dtos
{
    public class StatsDto
    {
        public int Users { get; set; }
        public int Posts{ get; set; }
        public int Comments { get; set; }
        public int Shares { get; set; }
        public int Reports { get; set; }
        public int Likes { get; set; }
        public int BlockedUsers { get; set; }
        public List<string> PopularTags { get; set; }
    }
}
