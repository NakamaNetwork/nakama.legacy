using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TreasureGuide.Entities;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer.DataParser
{
    public class UnitEvolutionParser : TreasureParser<IEnumerable<Tuple<int, int>>>
    {
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/evolutions.js";

        public UnitEvolutionParser(TreasureEntities context) : base(context, OptcDbUnitData)
        {
        }

        protected override IEnumerable<Tuple<int, int>> ConvertData(string trimmed)
        {
            var arrays = JsonConvert.DeserializeObject<JObject>(trimmed);
            var models = arrays.Children().SelectMany((line) =>
            {
                var id = line.Path?.ToInt32() ?? -1;
                var babbies = line.Children();
                var evo = babbies["evolution"];
                var moreChildren = evo.Children();
                if (moreChildren.Any())
                {
                    return moreChildren.Select(y => Tuple.Create(id, y.ToString().ToInt32() ?? -1));
                }
                return evo.Select(y => Tuple.Create(id, y.ToString().ToInt32() ?? -1));
            });
            var unique = models.GroupBy(x => String.Join(":::", x.Item1, x.Item2))
                .Select(x => x.First())
                .Where(x => x.Item1 > -1 && x.Item2 > -1)
                .ToList();
            return unique;
        }

        protected override async Task Save(IEnumerable<Tuple<int, int>> evolutions)
        {
            var i = 0;
            var count = evolutions.Count();
            Debug.WriteLine($"Writing {count} evolutions.");
            foreach (var evo in evolutions)
            {
                i++;
                var source = await Context.Units.SingleOrDefaultAsync(x => x.Id == evo.Item1);
                if (source != null)
                {
                    var target = await Context.Units.SingleOrDefaultAsync(x => x.Id == evo.Item2);
                    if (target != null)
                    {
                        source.EvolvesTo.Add(target);
                    }
                }
                if (i % 100 == 0)
                {
                    Debug.WriteLine($"    {i}/{count}");
                    await Context.SaveChangesAsync();
                }
            }
            Debug.WriteLine($"    {count}/{count}");
            await Context.SaveChangesAsync();
        }
    }
}
