using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NakamaNetwork.Entities.Models;
using NakamaNetwork.Sniffer.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NakamaNetwork.Sniffer.DataParser
{
    public class StageParser : TreasureParser<Tuple<List<Stage>, List<StageAlias>>>
    {
        private const string OptcDbStageData =
            "https://github.com/optc-db/optc-db.github.io/raw/master/common/data/drops.js";

        private HashSet<int> _existing;

        public StageParser(NakamaNetworkContext context) : base(context, OptcDbStageData)
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
                    if (output == null)
                    {
                        return Tuple.Create(new List<Stage>(), new List<StageAlias>());
                    }
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

        private Stage HandleSingle(string name, int? mainId, bool global, StageType stageType, int smallId = 0, int? thumb = null, bool bypassId = false)
        {
            var id = CreateId(stageType, mainId, smallId, bypassId);
            if (id == null)
            {
                return null;
            }
            thumb = thumb ?? mainId;
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
                Id = id.Value,
                UnitId = thumb,
                Name = name,
                Global = global,
                Type = stageType
            };
            return stage;
        }

        private int? CreateId(StageType stageType, int? thumb, int smallId, bool bypassId)
        {
            var idString = $"{(int)stageType}{thumb:0000}{smallId:00}";
            var id = Int32.Parse(idString);
            while (_existing.Contains(id))
            {
                if (bypassId)
                {
                    return null;
                }
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

            var doubles = HandleSingle("Double Character Missions", 1985, true, StageType.Special);
            stages.Add(doubles);
            aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Luffy Ace Missions Fortnight Event" });
            aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Lace Missions Fortnight Event" });
            aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Smoker Tashigi Missions Fortnight Event" });
            aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Double Character Introduction" });
        }

        private Tuple<List<Stage>, List<StageAlias>> HandleColiseum(JToken child)
        {
            var stageType = StageType.Coliseum;

            var eIds = GetIds(child, "Exhibition");
            var uIds = GetIds(child, "Underground");
            var cIds = GetIds(child, "Chaos");
            var nIds = GetIds(child, "Neo");

            var unitIds = eIds.Concat(uIds).Concat(cIds).Concat(nIds).Distinct().Join(Context.Units, x => x, y => y.Id, (id, unit) => id);
            var evos = GetBiggestEvos(unitIds);
            var all = evos.SelectMany(x => new[] {
                new StageCollectionDetail
                {
                    MainId = x.Item1,
                    ThumbId = x.Item2,
                    Name = " - Opening Stages",
                    SmallId = 0
                },
                new StageCollectionDetail
                {
                    MainId = x.Item1,
                    ThumbId = x.Item2,
                    Name = " - Final Stage",
                    SmallId = 1
                }
            });
            var units = all.Join(Context.Units, x => x.ThumbId, y => y.Id, (stage, unit) => Tuple.Create(unit, stage));
            var colo = units.Select(x => Tuple.Create(x.Item1,
                    HandleSingle($"Coliseum: {x.Item1.Name}{x.Item2.Name}", x.Item2.MainId,
                        x.Item1.Flags.HasFlag(UnitFlag.Global), stageType, x.Item2.SmallId, x.Item1.Id, true)))
                .Where(x => x.Item2 != null).ToList();
            var aliases = colo.SelectMany(GetAliases).ToList();
            var stages = colo.Select(x => x.Item2).ToList();
            return Tuple.Create(stages, aliases);
        }

        private IEnumerable<Tuple<int, int>> GetBiggestEvos(IEnumerable<int> unitIds)
        {
            return unitIds.GroupJoin(Context.UnitEvolutions, x => x, y => y.FromUnitId, (id, evo) => Tuple.Create(id, evo.LastOrDefault()?.ToUnitId ?? id));
        }

        private IEnumerable<int> GetIds(JToken child, string underground)
        {
            var token = child[underground];
            if (token != null)
            {
                return JsonConvert.DeserializeObject<int[]>(token.ToString()).AsEnumerable();
            }
            return Enumerable.Empty<int>();
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
            public int UnitName { get; set; }
            public int ThumbId { get; set; }
            public int MainId { get; set; }
            public string Name { get; set; }
            public int SmallId { get; set; }
        }
    }
}