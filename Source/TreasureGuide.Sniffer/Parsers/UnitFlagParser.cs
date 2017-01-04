using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TreasureGuide.Entities;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer.Parsers
{
    public class UnitFlagParser : TreasureParser<IEnumerable<UnitFlags>>
    {
        private const string OptcDbUnitFlagData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/flags.js";

        public UnitFlagParser(TreasureEntities context) : base(context, OptcDbUnitFlagData)
        {
        }

        protected override IEnumerable<UnitFlags> ConvertData(string trimmed)
        {
            var converted = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, bool>>>(trimmed);
            var results = converted.Select(x => new UnitFlags
            {
                UnitId = x.Key,
                Global = x.Value.GetSafe("global"),
                RR = x.Value.GetSafe("rr"),
                ERR = x.Value.GetSafe("rro"),
                LRR = x.Value.GetSafe("rrl"),
                Promo = x.Value.GetSafe("promo") || x.Value.GetSafe("special"),
                Shop = x.Value.GetSafe("shop")
            });
            return results;
        }

        protected override async Task Save(IEnumerable<UnitFlags> data)
        {
            Context.UnitFlags.AddRange(data);
            await Context.SaveChangesAsync();
        }
    }
}
