using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class ScheduleParserCal : TreasureParser<IEnumerable<ScheduledEvent>>
    {
        private readonly string Colos = "https://lukforce.bitbucket.io/optc-cal/app/calEvents/coliseumEvents.js";
        private readonly string Fortnights = "https://lukforce.bitbucket.io/optc-cal/app/calEvents/fortnightEvents.js";
        private readonly string Raids = "https://lukforce.bitbucket.io/optc-cal/app/calEvents/raidEvents.js";
        private readonly string Treasure = "https://lukforce.bitbucket.io/optc-cal/app/calEvents/tmEvents.js";
        private readonly string Special = "https://lukforce.bitbucket.io/optc-cal/app/calEvents/specialEvents.js";

        public ScheduleParserCal(TreasureEntities context) : base(context, null)
        {
        }

        protected override async Task<IEnumerable<ScheduledEvent>> GetData()
        {
            var raidData = await GetEventData(Raids, StageType.Raid);
            var fnData = await GetEventData(Fortnights, StageType.Fortnight);
            var coloData = await GetEventData(Colos, StageType.Coliseum);
            var treasureData = await GetEventData(Treasure, StageType.TreasureMap);
            var specialData = await GetEventData(Special, StageType.Special);

            var allData = raidData.Concat(fnData).Concat(coloData).Concat(treasureData).Concat(specialData).ToList();
            var grouped = allData.GroupBy(x => String.Join("__", x.StageId, x.Global, x.StartDate, x.EndDate))
                .Select(x => x.FirstOrDefault()).Where(x => x != null).ToList();

            return grouped;
        }

        private async Task<IEnumerable<ScheduledEvent>> GetEventData(string endpoint, StageType type)
        {
            var json = await PerformRequest(endpoint);
            var first = json.IndexOf("[");
            var last = json.LastIndexOf("]") + 1;
            var trimmed = json.Substring(first, last - first);
            var fixIt = Fix(trimmed);
            var stageData = JsonConvert.DeserializeObject<JArray>(fixIt);
            var stages = new List<ScheduledEvent>();
            foreach (var token in stageData)
            {
                var startDate = GetDate(token["start"]?.ToString());
                var endDate = GetDate(token["end"]?.ToString()) ?? startDate?.AddDays(1);
                if (startDate.HasValue)
                {
                    foreach (var unit in GetUnits(token))
                    {
                        var stageId = await GetStageId(unit, type);
                        if (stageId.HasValue)
                        {
                            var stage = new ScheduledEvent
                            {
                                StageId = stageId.Value,
                                StartDate = startDate.Value,
                                EndDate = endDate.Value,
                                Global = true,
                                Source = false
                            };
                            stages.Add(stage);
                        }
                        else
                        {
                            Debug.WriteLine("Zug");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Zug");
                }
            }
            return stages.ToList();
        }

        private string Fix(string trimmed)
        {
            trimmed = MultiCommentRegex.Replace(trimmed, "");
            trimmed = SingleCommentRegex.Replace(trimmed, "");
            return trimmed
                .Replace("ambush: ywb", "ambush: 1258")
                .Replace("ambush: shanks", "ambush: 1380")
                .Replace("ambush: cavendish", "ambush: 1530")
                .Replace("ambush: garp", "ambush: 1846")
                .Replace("id: 'sb_1023'", "id: 1023")
                .Replace("id: 'tp_1463'", "id: 1463")
                .Replace("id: 'tp_1465'", "id: 1463")
                .Replace("id: 'tp_1508'", "id: 1463")
                .Replace("id: 'tp_1516'", "id: 1463")
                .Replace("id: 'bb_0870'", "id: 870")
                .Replace("id: 'bb_1314'", "id: 1314")
                .Replace("id: 'bb_1404'", "id: 1404")
                .Replace("id: 'db_1985'", "id: 1985");
        }

        private DateTimeOffset? GetDate(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            DateTimeOffset date;
            if (DateTimeOffset.TryParse(value, out date))
            {
                date = new DateTimeOffset(date.Year, date.Month, date.Day, 3, 0, 0, TimeSpan.Zero).AddDays(1);
                return date;
            }
            return null;
        }

        private IEnumerable<int> GetUnits(JToken token)
        {
            return GetUnit(token["id"])
                .Concat(GetUnit(token["ambush"]))
                .Concat(GetUnit(token["newId"]))
                .Concat(GetUnit(token["repId"])).ToList();
        }

        private IEnumerable<int> GetUnit(JToken looper)
        {
            var ids = new List<int>();
            if (looper != null)
            {
                foreach (var loopId in looper)
                {
                    var id = loopId?.ToString()?.ToInt32();
                    if (id.HasValue)
                    {
                        ids.Add(id.Value);
                    }
                }
                var single = looper?.ToString()?.ToInt32();
                if (single.HasValue)
                {
                    ids.Add(single.Value);
                }
            }
            return ids.Distinct().ToList();
        }

        private async Task<int?> GetStageId(int unitId, StageType programType)
        {
            for (var i = 0; i < 2; i++)
            {
                var numeral = programType == StageType.Coliseum ? 1 : 0;
                var reps = new List<int> { unitId };
                do
                {
                    var id = TryGetStage(programType, numeral, reps);
                    if (id.HasValue)
                    {
                        return id;
                    }
                    reps = await Context.UnitEvolutions.Join(reps, x => x.FromUnitId, y => y, (x, y) => x.ToUnitId).ToListAsync();
                } while (reps.Any());
                reps = new List<int> { unitId };
                do
                {
                    var id = TryGetStage(programType, numeral, reps);
                    if (id.HasValue)
                    {
                        return id;
                    }
                    reps = await Context.UnitEvolutions.Join(reps, x => x.ToUnitId, y => y, (x, y) => x.FromUnitId).ToListAsync();
                } while (reps.Any());
                programType = StageType.Special;
            }
            return TryContingency(unitId);
        }

        private int? TryGetStage(StageType programType, int numeral, List<int> reps)
        {
            for (var i = reps.Count - 1; i >= 0; i--)
            {
                var rep = reps[i];
                var id = $"{(int)programType:00}{rep:0000}{numeral:00}"?.ToInt32();
                if (Context.Stages.Any(x => x.Id == id))
                {
                    return id;
                }
            }
            return null;
        }

        private int? TryContingency(int programIdentifier)
        {
            switch (programIdentifier)
            {
                // halloween
                case 1306:
                case 1326:
                    return 6130000;
                // summer
                case 681:
                case 685:
                case 690:
                case 1199:
                case 1201:
                case 1709:
                case 1711:
                    return 6068300;
                // valentines
                case 990:
                    return 6146300;
                // Mr. 3 and Buggy FN
                case 1302:
                    return 2130400;
                // Neptune
                case 1725:
                    return 2172300;
                // Raid Doffy v2
                case 2500:
                case 2262:
                case 2501:
                case 2263:
                case 5012:
                    return 4226300;
                case 1810:
                    return 2181200;
                case 1893:
                    return 2189100;
                case 1980:
                    return 2198200;
                case 1995:
                    return 2199700;
                case 2019:
                    return 2202100;
                case 2046:
                    return 2204400;
            }
            return null;
        }

        protected override async Task Save(IEnumerable<ScheduledEvent> items)
        {
            var remove = new List<ScheduledEvent>();
            foreach (var item in items)
            {
                if (Context.ScheduledEvents.Any(x => x.StageId == item.StageId
                    && x.Global == item.Global
                    && x.StartDate == item.StartDate
                    && x.EndDate == item.EndDate))
                {
                    remove.Add(item);
                }
            }
            items = items.Except(remove);
            await Context.LoopedAddSave(items);
        }
    }
}
