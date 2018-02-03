using System;

namespace TreasureGuide.Sniffer.Helpers
{
    public static class RedditExtensions
    {
        private const string FormatUri = "http://www.reddit.com/r/onepiecetc/comments/{0}/";

        public static Uri GetThreadUri(this string threadId)
        {
            var url = String.Format(FormatUri, threadId);
            return new Uri(url);
        }
    }
}
