using System;
using System.Collections;
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

        public static UnitTypeEnum? ToUnitType(this string input)
        {
            UnitTypeEnum value;
            if (UnitTypeEnum.TryParse(input, out value))
            {
                return value;
            }
            return null;
        }

        public static UnitClassEnum? ToUnitClass(this string input)
        {
            UnitClassEnum value;
            if (UnitClassEnum.TryParse(input, out value))
            {
                return value;
            }
            return null;
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
