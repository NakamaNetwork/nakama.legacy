using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string OptcDbStageData =
            "https://github.com/optc-db/optc-db.github.io/raw/master/common/data/drops.js";

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
            var stages = new List<Stage>();
            foreach (var datum in data)
            {
                var stageType = datum.Key.ToStageType();
                stages.AddRange(datum.Value.SelectMany(child =>
                {

                    var name = child["name"]?.ToString() ?? "Unknown";
                    if (stageType == StageType.Coliseum)
                    {
                        return HandleColiseum(child);
                    }
                    else if (stageType == StageType.TreasureMap)
                    {
                        name = "Treasure Map: " + name;
                    }
                    var global = child["global"]?.ToString().ToBoolean() ?? false;
                    int? thumb = null;
                    int thumbParse;
                    if (Int32.TryParse(child["thumb"]?.ToString() ?? "", out thumbParse))
                    {
                        thumb = thumbParse;
                    }
                    var output = HandleSingle(name, thumb, global, stageType);
                    return new[] { output };
                }));
            }

            return stages;
        }

        private Stage HandleSingle(string name, int? thumb, bool global, StageType stageType)
        {
            return new Stage
            {
                Id = IdMaker.FromString(name, (int)(stageType) * 1000000),
                UnitId = thumb,
                Name = name,
                Global = global,
                Type = stageType
            };
        }

        private IEnumerable<Stage> HandleColiseum(JToken child)
        {
            var stageType = StageType.Coliseum;
            var exhibition = JsonConvert.DeserializeObject<int[]>(child["Exhibition"].ToString())
                .SelectMany(x => Enumerable.Range(1, 3)
                    .Select(y => Tuple.Create(x, $" - Exhibition Stage {y}")));

            var underground = JsonConvert.DeserializeObject<int[]>(child["Underground"].ToString())
                .SelectMany(x => Enumerable.Range(1, 4)
                    .Select(y => Tuple.Create(x, $" - Underground Stage {y}")));

            var chaos = JsonConvert.DeserializeObject<int[]>(child["Chaos"].ToString())
                .SelectMany(x => Enumerable.Range(1, 5)
                    .Select(y => Tuple.Create(x, $" - Chaos Stage {y}")));

            var neo = JsonConvert.DeserializeObject<int[]>(child["Neo"].ToString())
                .SelectMany(x => Enumerable.Range(1, 5)
                    .Select(y => Tuple.Create(x, $" - Neo Stage {y}")));

            var all = exhibition.Concat(underground).Concat(chaos).Concat(neo).Distinct();
            var units = all.Join(Context.Units, x => x.Item1, y => y.Id,
                (stage, unit) => Tuple.Create(unit, stage.Item2));
            var colo = units.Select(x =>
                    HandleSingle($"Coliseum: {x.Item1.Name}{x.Item2}", x.Item1.Id,
                        x.Item1.Flags.HasFlag(UnitFlag.Global), stageType))
                .ToList();
            return colo;
        }

        protected override async Task Save(IEnumerable<Stage> items)
        {
            Context.Stages.Clear();
            foreach (var stage in items)
                Context.Stages.Add(stage);
            await Context.SaveChangesAsync();
        }
    }
}