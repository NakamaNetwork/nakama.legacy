using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
          var localAliases = new List<StageAlias>();
          if (stageType == StageType.Coliseum)
          {
            if (name == "Coliseum")
            {
              return HandleColiseumLegacy(child);
            }
            else
            {
              return HandleColiseum(name, child);
            }
          }
          else if (stageType == StageType.TreasureMap)
          {
            name = "Treasure Map: " + name;
          }
          else if (stageType == StageType.KizunaClash)
          {
            return HandleKizuna(name, child);
          }
          else if (stageType == StageType.Special && name.Contains("Garp Challenge"))
          {
            return Tuple.Create(new List<Stage>(), new List<StageAlias>());
          }
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
          var output = HandleSingle(name, thumb, global, stageType, localAliases);
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

    private Stage HandleSingle(string name, int? mainId, bool global, StageType stageType, List<StageAlias> aliases = null, int smallId = 0, int? thumb = null, bool bypassId = false)
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
      if (stageType == StageType.Special && (name.Contains("Ambush") || name.Contains("Invasion")))
      {
        aliases.Add(new StageAlias
        {
          StageId = id.Value,
          Name = name.Replace("Invasion", "Ambush")
        });
        aliases.Add(new StageAlias
        {
          StageId = id.Value,
          Name = name.Replace("Ambush", "Invasion")
        });
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
      var beli = HandleSingle("Golden Cave", 59, true, StageType.Weekly, aliases);
      stages.Add(beli);
      aliases.Add(new StageAlias { StageId = beli.Id, Name = "Beli Cavern" });

      var doubles = HandleSingle("Double Character Missions", 1985, true, StageType.Special, aliases);
      stages.Add(doubles);
      aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Luffy Ace Missions Fortnight Event" });
      aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Lace Missions Fortnight Event" });
      aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Smoker Tashigi Missions Fortnight Event" });
      aliases.Add(new StageAlias { StageId = doubles.Id, Name = "Double Character Introduction" });

      var coop = HandleSingle("Co-op Missions: Powerful Foes", 2161, true, StageType.Special, aliases);
      stages.Add(coop);
      aliases.Add(new StageAlias { StageId = coop.Id, Name = "co-op missions" });
      aliases.Add(new StageAlias { StageId = coop.Id, Name = "co-operative missions" });
      aliases.Add(new StageAlias { StageId = coop.Id, Name = "cooperative missions" });

      var coop2 = HandleSingle("Co-op Missions: Big Mom", 2234, true, StageType.Special, aliases);
      stages.Add(coop2);
      aliases.Add(new StageAlias { StageId = coop2.Id, Name = "co-op missions" });
      aliases.Add(new StageAlias { StageId = coop2.Id, Name = "co-operative missions" });
      aliases.Add(new StageAlias { StageId = coop2.Id, Name = "cooperative missions" });

      var akainu = HandleSingle("3 Stamina Ranking! Vs Akainu", 1298, true, StageType.Special, aliases);
      stages.Add(akainu);
      aliases.Add(new StageAlias { StageId = akainu.Id, Name = "ranking akainu" });
      aliases.Add(new StageAlias { StageId = akainu.Id, Name = "3 stam akainu" });
      aliases.Add(new StageAlias { StageId = akainu.Id, Name = "challenge akainu" });

      var sanji = HandleSingle("3 Stamina Ranking! Vs Sanji", 1451, true, StageType.Special, aliases);
      stages.Add(sanji);
      aliases.Add(new StageAlias { StageId = sanji.Id, Name = "ranking sanji" });
      aliases.Add(new StageAlias { StageId = sanji.Id, Name = "3 stam sanji" });
      aliases.Add(new StageAlias { StageId = sanji.Id, Name = "challenge sanji" });

      var lecrap = HandleSingle("Restaraunt le Crap", 2150, true, StageType.Special, aliases);
      stages.Add(lecrap);
      aliases.Add(new StageAlias { StageId = lecrap.Id, Name = "Cotton Candy" });
      aliases.Add(new StageAlias { StageId = lecrap.Id, Name = "Forest le Crap" });

      var haloCora = HandleSingle("Monster Party: Dead", 1325, true, StageType.Special, aliases);
      stages.Add(haloCora);
      aliases.Add(new StageAlias { StageId = haloCora.Id, Name = "Haloween Corazon" });
      aliases.Add(new StageAlias { StageId = haloCora.Id, Name = "Halloween Corazon" });

      var haloSug = HandleSingle("Monster Party: Loli", 1305, true, StageType.Special, aliases);
      stages.Add(haloSug);
      aliases.Add(new StageAlias { StageId = haloSug.Id, Name = "Haloween Sugar" });
      aliases.Add(new StageAlias { StageId = haloSug.Id, Name = "Halloween Sugar" });

      var haloKat = HandleSingle("Sweet and Spooky Party: Bitter", 2293, true, StageType.Special, aliases);
      stages.Add(haloKat);
      aliases.Add(new StageAlias { StageId = haloKat.Id, Name = "Haloween Katakuri" });
      aliases.Add(new StageAlias { StageId = haloKat.Id, Name = "Halloween Katakuri" });

      var sweetPudding = HandleSingle("Sweet Heart Memory - Pudding", 1963, true, StageType.Special, aliases);
      stages.Add(sweetPudding);
      aliases.Add(new StageAlias { StageId = sweetPudding.Id, Name = "Sweetheart Pudding" });
      aliases.Add(new StageAlias { StageId = sweetPudding.Id, Name = "Valentines Pudding" });
      aliases.Add(new StageAlias { StageId = sweetPudding.Id, Name = "Wedding Pudding" });

      var xmasCav = HandleSingle("Ambush: Christmas Cavendish", 1889, true, StageType.Special, aliases);
      stages.Add(xmasCav);
      aliases.Add(new StageAlias { StageId = xmasCav.Id, Name = "Christmas Invasion" });
      aliases.Add(new StageAlias { StageId = xmasCav.Id, Name = "Xmas Invasion" });
      aliases.Add(new StageAlias { StageId = xmasCav.Id, Name = "X-Mas Cabbage" });

      var wanoKizuna = HandleSingle("Kizuna Clash: Zoro & Sanji - Wano Special", 2531, true, StageType.KizunaClash, aliases, 99);
      stages.Add(wanoKizuna);
      aliases.Add(new StageAlias { StageId = wanoKizuna.Id, Name = "Kizuna Round 4" });

      var worldClashKizuna = HandleSingle("Kizuna Clash: Sakazuki", 2578, true, StageType.KizunaClash, aliases, 99);
      stages.Add(worldClashKizuna);
      aliases.Add(new StageAlias { StageId = worldClashKizuna.Id, Name = "Kizuna Round 5" });
      aliases.Add(new StageAlias { StageId = worldClashKizuna.Id, Name = "Kizuna World Clash" });
      aliases.Add(new StageAlias { StageId = worldClashKizuna.Id, Name = "Kizuna Clash Akainu" });
      aliases.Add(new StageAlias { StageId = worldClashKizuna.Id, Name = "Kizuna Clash  Korea" });

      var wanoClashLZ = HandleSingle("Pirates Arriving in Land of Wano: Luffytaro & Zorojuro", 2802, true, StageType.Special, aliases, 99);
      stages.Add(wanoClashLZ);
      aliases.Add(new StageAlias { StageId = wanoClashLZ.Id, Name = "Wano Blitz" });

      var wapolThing = HandleSingle("Wapol's Assault", 2799, true, StageType.Special, aliases, 0);
      stages.Add(wapolThing);
      aliases.Add(new StageAlias { StageId = wapolThing.Id, Name = "Wapol Christmas Xmas Thing" });

      var hawkins = HandleSingle("Ranking! Vs Hawkins", 2982, true, StageType.Special, aliases);
      stages.Add(hawkins);
      aliases.Add(new StageAlias { StageId = hawkins.Id, Name = "ranking hawkins" });
      aliases.Add(new StageAlias { StageId = hawkins.Id, Name = "3 stam hawkins" });
      aliases.Add(new StageAlias { StageId = hawkins.Id, Name = "challenge hawkins" });

      var garpIcons = new[] {
        Tuple.Create(1318, "#1"),
        Tuple.Create(1317, "#2"),
        Tuple.Create(1281, "#3"),
        Tuple.Create(3339, "#4"),
        Tuple.Create(1846, "#5"),
        Tuple.Create(3340, "#6"),
        Tuple.Create(870, "vs. Doflamingo"),
        Tuple.Create(2561, "vs. The Revolutionary Army")
      };
      for (var i = 0; i < garpIcons.Length; i++)
      {
        var garp = HandleSingle($"Garp Challenge {garpIcons[i].Item2}", 1318, true, StageType.Special, aliases, i, garpIcons[i].Item1, true);
        stages.Add(garp);
      }
    }

    private Tuple<List<Stage>, List<StageAlias>> HandleColiseum(string name, JToken child)
    {
      var stageType = StageType.Coliseum;

      var eIds = GetIds(child, "Exhibition").Concat(GetIds(child, "Exebition"));
      var uIds = GetIds(child, "Underground");
      var cIds = GetIds(child, "Chaos");
      var nIds = new[] { GetIds(child, "Neo").Concat(GetIds(child, "All Difficulties")).DefaultIfEmpty(0).Max() };

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
                    Name = "",
                    SmallId = 1
                }
            }).Where(x => !nIds.Contains(x.ThumbId) || x.SmallId == 1);
      var units = all.Join(Context.Units, x => x.ThumbId, y => y.Id, (stage, unit) => Tuple.Create(unit, stage));
      var colo = units.Select(x => Tuple.Create(x.Item1,
              HandleSingle($"{name}{x.Item2.Name}", x.Item2.MainId,
                  x.Item1.Flags.HasFlag(UnitFlag.Global), stageType, null, x.Item2.SmallId, x.Item1.Id, true)))
          .Where(x => x.Item2 != null).ToList();
      var aliases = colo.SelectMany(GetAliases).ToList();
      var stages = colo.Select(x => x.Item2).ToList();
      return Tuple.Create(stages, aliases);
    }

    private Tuple<List<Stage>, List<StageAlias>> HandleColiseumLegacy(JToken child)
    {
      var stageType = StageType.Coliseum;

      var eIds = GetIds(child, "Exhibition").Concat(GetIds(child, "Exebition"));
      var uIds = GetIds(child, "Underground");
      var cIds = GetIds(child, "Chaos");
      var nIds = GetIds(child, "Neo").Concat(GetIds(child, "All Difficulties"));

      var unitIds = eIds.Concat(uIds).Concat(cIds).Concat(nIds).Where(x => x > 0).Distinct().Join(Context.Units, x => x, y => y.Id, (id, unit) => id);
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
                  x.Item1.Flags.HasFlag(UnitFlag.Global), stageType, null, x.Item2.SmallId, x.Item1.Id, true)))
          .Where(x => x.Item2 != null).ToList();
      var aliases = colo.SelectMany(GetAliases).ToList();
      var stages = colo.Select(x => x.Item2).ToList();
      return Tuple.Create(stages, aliases);
    }

    private Tuple<List<Stage>, List<StageAlias>> HandleKizuna(string name, JToken child)
    {
      var stages = new List<Stage>();
      var aliases = new List<StageAlias>();

      for (var n = 0; n < 100; n++)
      {
        var id = GetIds(child, "Round " + n).DefaultIfEmpty(0).Max();
        id = GetBiggestEvos(new[] { id }).Select(x => x.Item2).Max();
        if (id != 0)
        {
          var madeName = "Kizuna Clash: " + name + " - Round " + n;
          var stage = HandleSingle(madeName, id, true, StageType.KizunaClash, new List<StageAlias>{
                        new StageAlias { Name = madeName.Replace("Kizuna","Kisuna") },
                        new StageAlias { Name = madeName.Replace("Clash", "Kessan") },
                        new StageAlias { Name = madeName.Replace("Clash", "Kessen") },
                        new StageAlias { Name = madeName.Replace("Kizuna Clash", "Bond Battle") }
                    }, n - 1, id, false);
          stages.Add(stage);
        }
      }
      if (!stages.Any())
      {
        var madeName = "Kizuna Clash: " + name;
        int id;
        if (Int32.TryParse(child["thumb"]?.ToString() ?? "", out id))
        {
          id = GetBiggestEvos(new[] { id }).Select(x => x.Item2).Max();
          var stage = HandleSingle(madeName, id, true, StageType.KizunaClash, new List<StageAlias>{
                        new StageAlias { Name = madeName.Replace("Kizuna","Kisuna") },
                        new StageAlias { Name = madeName.Replace("Clash", "Kessan") },
                        new StageAlias { Name = madeName.Replace("Clash", "Kessen") },
                        new StageAlias { Name = madeName.Replace("Kizuna Clash", "Bond Battle") }
                    });
          stages.Add(stage);
        }
      }

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
      var me = GetAliases(tuple.Item2.Name, new[] { tuple.Item1 }, tuple.Item2, true);
      var to = GetAliases(tuple.Item2.Name, tuple.Item1.EvolvesTo.Select(x => x.EvolvesTo), tuple.Item2);
      var from = GetAliases(tuple.Item2.Name, tuple.Item1.EvolvesFrom.Select(x => x.EvolvesFrom), tuple.Item2);
      return to.Concat(from).Concat(me);
    }

    private IEnumerable<StageAlias> GetAliases(string name, IEnumerable<Unit> targets, Stage detailItem2, bool ignoreMe = false)
    {
      var aliases = targets.Select(x => new StageAlias
      {
        StageId = detailItem2.Id,
        Name = x.Name
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

      var distinctA = items.Item1.GroupBy(x => x.Id).Select(y => y.Last()).ToList();

      await Context.LoopedAddSave(distinctA);

      var distinctB = items.Item2.GroupBy(x => $"{x.Stage}::::{x.Name}").Select(y => y.First()).ToList();

      await Context.LoopedAddSave(distinctB);
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