using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NakamaNetwork.Entities.Helpers;
using NakamaNetwork.Entities.Models;
using NakamaNetwork.Sniffer.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NakamaNetwork.Sniffer.DataParser
{
    public class UnitEvolutionParser : TreasureParser<IEnumerable<UnitEvolution>>
    {
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/evolutions.js";

        public UnitEvolutionParser(NakamaNetworkContext context) : base(context, OptcDbUnitData)
        {
        }

        protected override IEnumerable<UnitEvolution> ConvertData(string trimmed)
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
            var evos = unique.Select(x => new UnitEvolution
            {
                FromUnitId = x.Item1,
                ToUnitId = x.Item2
            }).ToList();
            return evos;
        }

        protected override async Task Save(IEnumerable<UnitEvolution> evolutions)
        {
            Context.UnitEvolutions.Clear();
            await Context.LoopedAddSave(evolutions);
        }
    }
}
