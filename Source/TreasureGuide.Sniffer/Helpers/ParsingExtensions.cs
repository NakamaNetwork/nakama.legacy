using System;

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
    }
}
