using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer.Parsers
{
    public class StageParser : TreasureParser<IEnumerable<Stage>>
    {
        private const string OptcDbStageData = "https://github.com/optc-db/optc-db.github.io/raw/master/common/data/drops.js";

        public StageParser(TreasureEntities context) : base(context, OptcDbStageData)
        {
        }

        protected override string TrimData(string input)
        {
            var start = input.IndexOf('=') + 1;
            var end = input.IndexOf(';', input.IndexOf("var bonuses") - 10);
            return input.Substring(start, end - start);
        }

        protected override IEnumerable<Stage> ConvertData(string trimmed)
        {
            var data = JsonConvert.DeserializeObject<JObject>(trimmed);
            var i = 0;
            var stages = new List<Stage>();
            foreach (var datum in data)
            {
                var stageType = datum.Key.ToStageType();
                stages.AddRange(datum.Value.SelectMany(child =>
                {
                    var name = child["name"]?.ToString() ?? "Unknown";
                    if (name == "Coliseum")
                    {
                        return HandleColiseum(child);
                    }
                    else
                    {
                        var global = child["global"]?.ToString().ToBoolean() ?? false;
                        return new[] { HandleSingle(name, global, stageType) };
                    }
                }));
            }

            return stages;
        }
        
        private Stage HandleSingle(string name, bool global, StageType stageType)
        {
            return new Stage
            {
                Id = IdMaker.FromString(name),
                Name = name,
                Global = global,
                Type = stageType
            };
        }

        private IEnumerable<Stage> HandleColiseum(JToken child)
        {
            var stageType = StageType.Coliseum;
            var exhibition = JsonConvert.DeserializeObject<int[]>(child["Exhibition"].ToString());
            var underground = JsonConvert.DeserializeObject<int[]>(child["Underground"].ToString());
            var chaos = JsonConvert.DeserializeObject<int[]>(child["Chaos"].ToString());
            var all = exhibition.Concat(underground).Concat(chaos).Distinct();
            var units = all.Join(Context.Units, x => x, y => y.Id, (id, unit) => unit);
            var colo = units.Select(x => HandleSingle("Coliseum: " + x.Name, x.Flags.HasFlag(UnitFlag.Global), stageType))
                .ToList();
            return colo;
        }

        protected override async Task Save(IEnumerable<Stage> items)
        {
            Context.Stages.Clear();
            foreach (var stage in items)
            {
                Context.Stages.Add(stage);
            }
            await Context.SaveChangesAsync();
        }
    }
}
