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
    public class StageParser : TreasureParser<IEnumerable<ParsedStageModel>>
    {
        private const string OptcDbDropData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/drops.js";
        private static readonly string[] KnownKeys = { "name", "shortName", "thumb", "completion", "global", "completion units", "condition", "challenge", "challengeData" };

        public StageParser(TreasureEntities context) : base(context, OptcDbDropData)
        {
        }

        protected override string TrimData(string input)
        {
            var start = input.IndexOf('=') + 1;
            var end = input.LastIndexOf("var bonuses");
            var sliced = input.Substring(start, end - start).Trim();
            return base.TrimData(sliced);
        }

        protected override IEnumerable<ParsedStageModel> ConvertData(string trimmed)
        {
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<Dictionary<string, object>>>>(trimmed);
            var models = deserialized.SelectMany((x, i) => x.Value.Select((y, j) =>
            {
                var stage = new Stage
                {
                    Name = y.GetSafe("name", "Unknown").ToString(),
                    Global = y.GetSafe("global", false).ToString().ToBoolean() ?? false,
                    Type = GetStageType(x.Key)
                };
                return new ParsedStageModel
                {
                    Stage = stage,
                    StageDifficulties = y.Keys.Except(KnownKeys).Select((z, k) => new StageDifficulty
                    {
                        Stage = stage,
                        Stamina = 0,
                        Name = z,
                        Global = stage.Global
                    })
                };
            }));
            return models;
        }

        private StageType GetStageType(string key)
        {
            switch (key)
            {
                case "Story Island":
                    return StageType.Story;
                case "Weekly Island":
                    return StageType.Weekly;
                case "Fortnight":
                    return StageType.Fortnight;
                case "Raid":
                    return StageType.Raid;
                case "Special":
                    return StageType.Special;
                default:
                    return StageType.Story;
            }
        }

        protected override async Task Save(IEnumerable<ParsedStageModel> items)
        {
            Context.Stages.Clear();
            Context.StageDifficulties.Clear();
            foreach (var stage in items)
            {
                Context.Stages.Add(stage.Stage);
                Context.StageDifficulties.AddRange(stage.StageDifficulties);
            }
            await Context.SaveChangesAsync();
        }
    }

    public class ParsedStageModel
    {
        public Stage Stage { get; set; }
        public IEnumerable<StageDifficulty> StageDifficulties { get; set; }
    }
}
