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
            var converted = new List<Tuple<StageType, Dictionary<string, string>[]>>();
            var i = 0;
            var stages = new List<Stage>();
            foreach (var datum in data)
            {
                var stageType = datum.Key.ToStageType();
                stages.AddRange(datum.Value.Select(child => new Stage
                {
                    Id = ++i,
                    Name = child["name"]?.ToString() ?? "Unknown",
                    Global = child["global"]?.ToString().ToBoolean() ?? false,
                    Type = stageType
                }));
            }
            return stages;
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
