using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer.DataParser
{
    public class StageParser : TreasureParser<Tuple<List<Stage>, List<StageAlias>>>
    {
        private const string OptcDbStageData =
            "https://github.com/optc-db/optc-db.github.io/raw/master/common/data/drops.js";

        private HashSet<int> _existing;

        public StageParser(TreasureEntities context) : base(context, OptcDbStageData)
        {
            _existing = new HashSet<int>();
        }

        protected override string TrimData(string input)
        {
            var start = input.IndexOf('=') + 1;
            var end = input.IndexOf(';', input.IndexOf("var bonuses") - 10);
            return input.Substring(start, end - start);
        }

        protected override Tuple<List<Stage>, List<StageAlias>> ConvertData(string trimmed)
        {
            var data = JsonConvert.DeserializeObject<JObject>(trimmed);
            var stages = new List<Stage>();
            var aliases = new List<StageAlias>();
            foreach (var datum in data)
            {
                var stageType = datum.Key.ToStageType();
                var datas = datum.Value.Select(child =>
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
                    return Tuple.Create(new List<Stage> { output }, new List<StageAlias>());
                }).ToList();
                stages.AddRange(datas.SelectMany(x => x.Item1).ToList());
                aliases.AddRange(datas.SelectMany(x => x.Item2).ToList());
            }

            return Tuple.Create(stages, aliases);
        }

        private Stage HandleSingle(string name, int? thumb, bool global, StageType stageType, int smallId = 0)
        {
            var id = CreateId(stageType, thumb, smallId);
            var oldId = IdMaker.FromString(name, (int)(stageType) * 1000000);
            var stage = new Stage
            {
                Id = id,
                UnitId = thumb,
                Name = name,
                Global = global,
                Type = stageType,
                OldId = oldId,
            };
            return stage;
        }

        private int CreateId(StageType stageType, int? thumb, int smallId)
        {
            var idString = $"{(int)stageType}{thumb:0000}{smallId:00}";
            var id = Int32.Parse(idString);
            while (_existing.Contains(id))
            {
                id++;
            }
            _existing.Add(id);
            return id;
        }

        private Tuple<List<Stage>, List<StageAlias>> HandleColiseum(JToken child)
        {
            var stageType = StageType.Coliseum;
            var exhibition = JsonConvert.DeserializeObject<int[]>(child["Exhibition"].ToString())
                .SelectMany(x => Enumerable.Range(1, 3).Select(y => new StageCollectionDetail
                {
                    UnitId = x,
                    Name = $" - Exhibition Stage {y}",
                    SmallId = y
                }));

            var underground = JsonConvert.DeserializeObject<int[]>(child["Underground"].ToString())
                .SelectMany(x => Enumerable.Range(1, 4)
                    .Select(y => new StageCollectionDetail
                    {
                        UnitId = x,
                        Name = $" - Underground Stage {y}",
                        SmallId = 10 + y
                    }));

            var chaos = JsonConvert.DeserializeObject<int[]>(child["Chaos"].ToString())
                .SelectMany(x => Enumerable.Range(1, 5)
                    .Select(y => new StageCollectionDetail
                    {
                        UnitId = x,
                        Name = $" - Chaos Stage {y}",
                        SmallId = 20 + y
                    }));

            var neo = JsonConvert.DeserializeObject<int[]>(child["Neo"].ToString())
                .SelectMany(x => Enumerable.Range(1, 5)
                    .Select(y => new StageCollectionDetail
                    {
                        UnitId = x,
                        Name = $" - Neo Stage {y}",
                        SmallId = 30 + y
                    }));

            var all = exhibition.Concat(underground).Concat(chaos).Concat(neo).Distinct();
            var units = all.Join(Context.Units, x => x.UnitId, y => y.Id, (stage, unit) => Tuple.Create(unit, stage));
            var colo = units.Select(x => Tuple.Create(x.Item1,
                    HandleSingle($"Coliseum: {x.Item1.Name}{x.Item2.Name}", x.Item1.Id,
                        x.Item1.Flags.HasFlag(UnitFlag.Global), stageType, x.Item2.SmallId)))
                .ToList();
            var aliases = colo.SelectMany(GetAliases).ToList();
            var stages = colo.Select(x => x.Item2).ToList();
            return Tuple.Create(stages, aliases);
        }

        private IEnumerable<StageAlias> GetAliases(Tuple<Unit, Stage> tuple)
        {
            var me = GetAliases(tuple.Item1.Name, new[] { tuple.Item1 }, tuple.Item2, true);
            var to = GetAliases(tuple.Item1.Name, tuple.Item1.EvolvesTo, tuple.Item2);
            var from = GetAliases(tuple.Item1.Name, tuple.Item1.EvolvesFrom, tuple.Item2);
            return to.Concat(from).Concat(me);
        }

        private IEnumerable<StageAlias> GetAliases(string name, ICollection<Unit> targets, Stage detailItem2, bool ignoreMe = false)
        {
            var aliases = targets.Select(x => new StageAlias
            {
                StageId = detailItem2.Id,
                Name = detailItem2.Name.Replace(name, x.Name)
            });
            var other = aliases.Select(x => new StageAlias
            {
                StageId = x.StageId,
                Name = x.Name.Replace("Coliseum", "Colosseum")
            });
            return ignoreMe ? other : other.Concat(aliases);
        }

        protected override async Task Save(Tuple<List<Stage>, List<StageAlias>> items)
        {
            Context.StageAliases.Clear();

            Context.Stages.Clear();

            await Context.LoopedAddSave(items.Item1);

            await Context.LoopedAddSave(items.Item2);
        }

        private class StageCollectionDetail
        {
            public int UnitId { get; set; }
            public string Name { get; set; }
            public int SmallId { get; set; }
        }
    }
}