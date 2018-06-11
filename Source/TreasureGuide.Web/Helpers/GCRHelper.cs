using System;
using System.Text.RegularExpressions;

namespace TreasureGuide.Common.Helpers
{
    public static class GCRHelper
    {
        private static readonly Regex AokijiRegex = new Regex("057[54]");

        public static string GetIcon(int? unitId)
        {
            if (unitId.HasValue)
            {
                var id = AokijiRegex.Replace($"{unitId:0000}", "0$0"); // missing aokiji image
                if (id == "0742")
                {
                    return "https://onepiece-treasurecruise.com/wp-content/uploads/f0742-2.png";
                }
                if (id == "2200")
                {
                    return "http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5011.png";
                }
                if (id == "2201")
                {
                    return "http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5012.png";
                }
                return $"https://onepiece-treasurecruise.com/wp-content/uploads/f{id}.png";
            }
            return null;
        }

        public static string GetBackgroundStyle(int? unitId)
        {
            var style = "background-image: ";
            var icon = GetIcon(unitId);
            if (!String.IsNullOrWhiteSpace(icon))
            {
                style += $"url('{icon}'), ";
            }
            style += "url('https://onepiece-treasurecruise.com/wp-content/themes/onepiece-treasurecruise/images/noimage.png\')";
            return style;
        }

        public static string GetShortStageName(string stageName)
        {
            stageName = stageName.Replace("Clash!!", "Raid");
            stageName = stageName.Replace("Clash Neo!!", "Neo");
            stageName = stageName.Replace("Coliseum:", "Colo");
            stageName = stageName.Replace("Invasion!", "Inv");
            stageName = stageName.Replace("Forest of Training:", "Forest");
            return stageName;
        }
    }
}
