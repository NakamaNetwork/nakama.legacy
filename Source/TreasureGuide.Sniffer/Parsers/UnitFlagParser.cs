using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer.Parsers
{
    public class UnitFlagParser : TreasureParser<IEnumerable<UnitFlag>>
    {
        private const string OptcDbUnitFlagData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/flags.js";

        public UnitFlagParser(TreasureEntities context) : base(context, OptcDbUnitFlagData)
        {
        }

        protected override IEnumerable<UnitFlag> ConvertData(string trimmed)
        {
            var converted = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, bool>>>(trimmed);
            var results = converted.SelectMany(x => x.Value.Where(y => y.Value)
            .Select(y => GetFlagType(y.Key))
            .Where(y => y.HasValue)
            .Select(y => new UnitFlag
            {
                UnitId = x.Key,
                FlagType = y.Value
            }))
            .GroupBy(x => Tuple.Create(x.UnitId, x.FlagType))
            .Select(x => x.First());
            return results;
        }

        private UnitFlagTypes? GetFlagType(string value)
        {
            switch (value.ToLower())
            {
                case "global":
                    return UnitFlagTypes.Global;
                case "rr":
                    return UnitFlagTypes.RareRecruit;
                case "rro":
                    return UnitFlagTypes.RareRecruitExclusive;
                case "rrl":
                    return UnitFlagTypes.RareRecruitLimited;
                case "promo":
                case "special":
                    return UnitFlagTypes.Promotional;
                case "shop":
                    return UnitFlagTypes.Shop;
                default:
                    return null;
            }
        }

        protected override async Task Save(IEnumerable<UnitFlag> items)
        {
            Context.UnitFlags.Clear();
            foreach (var datum in items)
            {
                Context.UnitFlags.Add(datum);
            }
            await Context.SaveChangesAsync();
        }
    }
}
