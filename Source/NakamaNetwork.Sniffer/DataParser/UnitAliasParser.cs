using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NakamaNetwork.Entities.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NakamaNetwork.Sniffer.DataParser
{
    public class UnitAliasParser : TreasureParser<IEnumerable<UnitAlias>>
    {
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/aliases.js";

        public UnitAliasParser(NakamaNetworkContext context) : base(context, OptcDbUnitData)
        {
        }

        protected override IEnumerable<UnitAlias> ConvertData(string trimmed)
        {
            var arrays = JsonConvert.DeserializeObject<JObject>(trimmed);
            var models = arrays.Values().SelectMany((line) =>
            {
                return line.Children().Select(y => new UnitAlias
                {
                    UnitId = Int32.Parse(line.Path),
                    Name = y.Value<string>()
                });
            });
            var unique = models.GroupBy(x => String.Join(":::", x.UnitId, x.Name)).Select(x => x.First()).Where(x => !String.IsNullOrWhiteSpace(x.Name)).ToList();
            return unique;
        }

        protected override async Task Save(IEnumerable<UnitAlias> aliases)
        {
            aliases = aliases.Join(Context.Units, x => x.UnitId, y => y.Id, (x, y) => y != null ? x : null).Where(x => x != null);
            Context.UnitAliases.Clear();
            await Context.LoopedAddSave(aliases);
        }
    }
}
