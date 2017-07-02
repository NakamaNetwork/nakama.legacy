using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TreasureGuide.Sniffer.Helpers
{
    public static class IdMaker
    {
        private static readonly Regex CleanUpRegex = new Regex(@"^(\w\d)");
        private static readonly IDictionary<int, string> Usages = new Dictionary<int, string>();

        public static int FromString(string text, int addition = 0)
        {
            text = CleanUpRegex.Replace(text, "").ToLower();
            var length = text.Length;
            var id = text.Select((c, i) => c * 31 ^ (length - i)).Sum();
            id += addition;

            if (Usages.ContainsKey(id))
            {
                throw new Exception();
            }
            Usages.Add(id, text);
            return id;
        }
    }
}
