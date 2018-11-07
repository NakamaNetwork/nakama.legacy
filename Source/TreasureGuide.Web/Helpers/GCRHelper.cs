﻿using System;
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
                switch (id)
                {
                    case "0742": return "https://onepiece-treasurecruise.com/wp-content/uploads/f0742-2.png";
                    case "2500": case "2200": case "2262": return "http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5011.png";
                    case "2501": case "2201": case "2263": return "http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5012.png";
                    case "2502": return "http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5013.png";
                    case "2503": return "http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5014.png";
                    case "2504": return "http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5015.png";
                }
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
