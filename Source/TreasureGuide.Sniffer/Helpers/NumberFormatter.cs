using System;
using System.Linq;

namespace TreasureGuide.Sniffer.Helpers
{
    public static class NumberFormatter
    {
        public static int Pad(int spacing, params int[] numbers)
        {
            return Int32.Parse(String.Join("", numbers.Select(x => x.ToString("D" + spacing))));
        }
    }
}
