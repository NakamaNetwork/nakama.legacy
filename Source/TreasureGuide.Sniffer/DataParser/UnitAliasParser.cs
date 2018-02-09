using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;

namespace TreasureGuide.Sniffer.DataParser
{
    public class UnitAliasParser : TreasureParser<IEnumerable<UnitAlias>>
    {
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/aliases.js";

        public UnitAliasParser(TreasureEntities context) : base(context, OptcDbUnitData)
        {
        }

        protected override IEnumerable<UnitAlias> ConvertData(string trimmed)
        {
            var arrays = JsonConvert.DeserializeObject<JObject>(trimmed);
            var models = arrays.Values().SelectMany((line) =>
            {
                return line.Children().Select(y =>
                {
                    return new UnitAlias
                    {
                        UnitId = Int32.Parse(line.Path),
                        Name = y.Value<string>(),
                    };
                });
            });
            var unique = models.GroupBy(x => String.Join(":::", x.UnitId, x.Name)).Select(x => x.First()).Where(x => !String.IsNullOrWhiteSpace(x.Name)).ToList();
            return unique;
        }

        protected override async Task Save(IEnumerable<UnitAlias> aliases)
        {
            Context.UnitAliases.Clear();
            foreach (var alias in aliases)
            {
                Context.UnitAliases.Add(alias);
            }
            await Context.SaveChangesAsync();
        }
    }
}
