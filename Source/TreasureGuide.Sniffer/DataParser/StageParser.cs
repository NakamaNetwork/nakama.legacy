using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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
                    var localAliases = new List<StageAlias>();
                    var global = child["global"]?.ToString().ToBoolean() ?? false;
                    int? thumb = null;
                    int thumbParse;
                    if (Int32.TryParse(child["thumb"]?.ToString() ?? "", out thumbParse))
                    {
                        var unit = Context.Units.SingleOrDefault(x => x.Id == thumbParse);
                        if (unit != null)
                        {
                            thumb = thumbParse;
                            localAliases.Add(new StageAlias { Name = unit.Name + " " + stageType });
                        }
                    }
                    var output = HandleSingle(name, thumb, global, stageType);
                    foreach (var alias in localAliases)
                    {
                        alias.StageId = output.Id;
                    }
                    return Tuple.Create(new List<Stage> { output }, localAliases);
                }).ToList();
                stages.AddRange(datas.SelectMany(x => x.Item1).ToList());
                aliases.AddRange(datas.SelectMany(x => x.Item2).ToList());
            }
            CreateExtras(stages, aliases);

            return Tuple.Create(stages, aliases);
        }

        private Stage HandleSingle(string name, int? thumb, bool global, StageType stageType, int smallId = 0)
        {
            var id = CreateId(stageType, thumb, smallId);
            if (thumb.HasValue)
            {
                var unit = Context.Units.Any(x => x.Id == thumb);
                if (!unit)
                {
                    Debug.WriteLine($"Team {id} '{name}' thumbnail unit ({thumb}) was not found.");
                    thumb = null;
                }
            }
            var stage = new Stage
            {
                Id = id,
                UnitId = thumb,
                Name = name,
                Global = global,
                Type = stageType
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

        private void CreateExtras(List<Stage> stages, List<StageAlias> aliases)
        {
            var beli = HandleSingle("Golden Cave", 59, true, StageType.Weekly);
            stages.Add(beli);
            aliases.Add(new StageAlias { StageId = beli.Id, Name = "Beli Cavern" });
        }

        private Tuple<List<Stage>, List<StageAlias>> HandleColiseum(JToken child)
        {
            var stageType = StageType.Coliseum;

            var eIds = JsonConvert.DeserializeObject<int[]>(child["Exhibition"].ToString()).AsEnumerable();
            var uIds = JsonConvert.DeserializeObject<int[]>(child["Underground"].ToString()).AsEnumerable();
            var cIds = JsonConvert.DeserializeObject<int[]>(child["Chaos"].ToString()).AsEnumerable();
            var nIds = JsonConvert.DeserializeObject<int[]>(child["Neo"].ToString()).AsEnumerable();

            var all = eIds.Concat(uIds).Concat(cIds).Concat(nIds).Distinct().SelectMany(x => new[] {
                new StageCollectionDetail
                {
                    UnitId = x,
                    Name = " - Opening Stages",
                    SmallId = 0
                },
                new StageCollectionDetail
                {
                    UnitId = x,
                    Name = " - Final Stage",
                    SmallId = 1
                }
            });
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
            var to = GetAliases(tuple.Item1.Name, tuple.Item1.EvolvesTo.Select(x => x.EvolvesTo), tuple.Item2);
            var from = GetAliases(tuple.Item1.Name, tuple.Item1.EvolvesFrom.Select(x => x.EvolvesFrom), tuple.Item2);
            return to.Concat(from).Concat(me);
        }

        private IEnumerable<StageAlias> GetAliases(string name, IEnumerable<Unit> targets, Stage detailItem2, bool ignoreMe = false)
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