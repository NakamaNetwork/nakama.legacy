using System;
using System.Collections.Generic;
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

        public static UnitFlag? ToFlagType(this string value)
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
    }
}
