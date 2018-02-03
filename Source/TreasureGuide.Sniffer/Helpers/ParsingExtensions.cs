using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TreasureGuide.Entities;

namespace TreasureGuide.Sniffer.Helpers
{
    public static class ParsingExtensions
    {
        public static Byte? ToByte(this string input)
        {
            Byte value;
            if (Byte.TryParse(input, out value))
            {
                return value;
            }
            return null;
        }

        public static Int16? ToInt16(this string input)
        {
            Int16 value;
            if (Int16.TryParse(input, out value))
            {
                return value;
            }
            return null;
        }

        public static Int32? ToInt32(this string input)
        {
            Int32 value;
            if (Int32.TryParse(input, out value))
            {
                return value;
            }
            return null;
        }

        public static Decimal? ToDecimal(this string input)
        {
            Decimal value;
            if (Decimal.TryParse(input, out value))
            {
                return value;
            }
            return null;
        }

        public static Boolean? ToBoolean(this string input)
        {
            Boolean value;
            if (Boolean.TryParse(input, out value))
            {
                return value;
            }
            return null;
        }

        public static UnitType ToUnitType(this string input)
        {
            UnitType value;
            if (UnitType.TryParse(input, out value))
            {
                return value;
            }
            return UnitType.Unknown;
        }

        public static StageType ToStageType(this string value)
        {
            switch (value.ToLower())
            {
                case "story island":
                    return StageType.Story;
                case "weekly island":
                    return StageType.Weekly;
                case "fortnight":
                    return StageType.Fortnight;
                case "raid":
                    return StageType.Raid;
                case "special":
                    return StageType.Special;
                case "coliseum":
                    return StageType.Coliseum;
                case "training forest":
                    return StageType.TrainingForest;
                case "treasure map":
                    return StageType.TreasureMap;
                default:
                    return StageType.Unknown;
            }
        }

        public static UnitFlag ToFlagType(this string value)
        {
            switch (value.ToLower())
            {
                case "global":
                    return UnitFlag.Global;
                case "rr":
                    return UnitFlag.RareRecruit;
                case "rro":
                case "orr":
                    return UnitFlag.RareRecruitExclusive;
                case "rrl":
                case "lrr":
                    return UnitFlag.RareRecruitLimited;
                case "promo":
                case "special":
                    return UnitFlag.Promotional;
                case "shop":
                    return UnitFlag.Shop;
                default:
                    return UnitFlag.Unknown;
            }
        }

        public static UnitClass ToUnitClass(this string input)
        {
            UnitClass value;
            if (UnitClass.TryParse(input.Replace(" ", ""), out value))
            {
                return value;
            }
            return UnitClass.Unknown;
        }

        public static TValue GetSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue fallback = default(TValue))
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            return fallback;
        }

        private static readonly Regex CalcRegex = new Regex("transfer/(.+?)H", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public static IEnumerable<string> GetCalcLinks(this string body)
        {
            var matches = CalcRegex.Matches(body);
            var links = new List<string>();
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                links.Add($"http://optc-db.github.io/damage/#/{match.Value}");
            }
            return links;
        }

        private static readonly Regex VidRegex = new Regex(@"(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|[a-zA-Z0-9_\-]+\?v=)([^#\&\?\n<>\'\""\s\)]*)", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static IEnumerable<string> GetYouTubeLinks(this string body)
        {
            var matches = VidRegex.Matches(body);
            var links = new List<string>();
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i].Value;
                var start = Math.Max(0, match.LastIndexOf("/") + 1);
                var end = Math.Max(0, match.IndexOf(" "));
                var length = (end == 0 ? match.Length : end) - start;
                var value = length > 0 ? match.Substring(start, length) : match;
                links.Add($"http://youtu.be/{value}");
            }
            return links;
        }
    }
}
