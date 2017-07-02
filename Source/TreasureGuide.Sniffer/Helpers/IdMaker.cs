using System;
using System.Collections.Generic;

namespace TreasureGuide.Sniffer.Helpers
{
    public static class IdMaker
    {
        private static readonly HashSet<int> AlreadyUsed = new HashSet<int>();

        public static int FromString(string text)
        {
            var id = Math.Abs(text.GetHashCode());
            if (AlreadyUsed.Contains(id))
            {
                // Add another hash from the middle of the text.
                var start = (int)(text.Length * 0.3);
                var end = (int)(text.Length * 0.6);
                var addition = Math.Abs(text.Substring(start, end - start).GetHashCode());
                id += addition;
            }
            if (AlreadyUsed.Contains(id))
            {
                throw new Exception("This Id generation is stupid!");
            }
            AlreadyUsed.Add(id);
            return id;
        }
    }
}
