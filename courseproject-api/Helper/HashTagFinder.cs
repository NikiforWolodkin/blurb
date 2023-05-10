using System.Text.RegularExpressions;

namespace courseproject_api.Helper
{
    public static class HashtagFinder
    {
        public static ICollection<string> FindHashtags(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"(?<=#)\w+");

            List<string> hashtags = new List<string>();

            foreach (Match match in matches)
            {
                hashtags.Add(match.Value);
            }

            return hashtags;
        }

        public static string RemoveHashtags(string input)
        {
            return Regex.Replace(input, @"#\w+", "");
        }
    }
}