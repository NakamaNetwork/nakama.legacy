using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NakamaNetwork.Entities.Models;
using NakamaNetwork.Sniffer.Helpers;
using Newtonsoft.Json;

namespace NakamaNetwork.Sniffer.DataParser
{
    public class UnitFlagParser : TreasureParser<IEnumerable<UnitFlagModel>>
    {
        private const string OptcDbUnitFlagData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/flags.js";

        public UnitFlagParser(NakamaNetworkContext context) : base(context, OptcDbUnitFlagData)
        {
        }

        protected override IEnumerable<UnitFlagModel> ConvertData(string trimmed)
        {
            var converted = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, bool>>>(trimmed);
            var results = converted.SelectMany(x => x.Value.Where(y => y.Value)
            .Select(y => Tuple.Create(x.Key, y.Key.ToFlagType())))
            .GroupBy(x => x.Item1, x => x.Item2)
            .Select(x => new UnitFlagModel
            {
                UnitId = x.Key,
                Flags = x.Aggregate(UnitFlag.Unknown, (current, parsed) => current | parsed)
            });
            return results;
        }

        protected override async Task Save(IEnumerable<UnitFlagModel> items)
        {
            var batch = 100;
            var current = 0;
            foreach (var datum in items)
            {
                var item = await Context.Units.SingleOrDefaultAsync(x => x.Id == datum.UnitId);
                if (item != null && item.Flags != datum.Flags)
                {
                    item.Flags = datum.Flags;
                    current++;
                    if (current >= batch)
                    {
                        current = 0;
                        await Context.SaveChangesAsync();
                    }
                }
            }
            await Context.SaveChangesAsync();
        }
    }

    public sealed class UnitFlagModel
    {
        public int UnitId { get; set; }
        public UnitFlag Flags { get; set; }
    }
}
