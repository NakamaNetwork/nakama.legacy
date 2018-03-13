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

namespace TreasureGuide.Sniffer.DataParser
{
    public class ScheduleParser : TreasureParser<IEnumerable<ScheduledEvent>>
    {
        private readonly string GlobalSchedule = "https://raw.githubusercontent.com/OPTC-Agenda/OPTC-Agenda.github.io/master/assets/json/weeks.json";
        private readonly string JapanSchedule = "https://raw.githubusercontent.com/OPTC-Agenda/OPTC-Agenda.github.io/master/assets/json/weeksJap.json";

        private readonly string Colos = "https://raw.githubusercontent.com/OPTC-Agenda/OPTC-Agenda.github.io/master/assets/json/colo.json";
        private readonly string Fortnights = "https://raw.githubusercontent.com/OPTC-Agenda/OPTC-Agenda.github.io/master/assets/json/fn.json";
        private readonly string Raids = "https://raw.githubusercontent.com/OPTC-Agenda/OPTC-Agenda.github.io/master/assets/json/raid.json";
        private readonly string Specials = "https://raw.githubusercontent.com/OPTC-Agenda/OPTC-Agenda.github.io/master/assets/json/special.json";

        private readonly string Drops = "https://raw.githubusercontent.com/OPTC-Agenda/OPTC-Agenda.github.io/master/assets/json/drops.json";

        public ScheduleParser(TreasureEntities context) : base(context, null)
        {
        }

        protected override async Task<IEnumerable<ScheduledEvent>> GetData()
        {
            var dropData = await GetDropData();

            var specialData = await GetEventData(Specials, StageType.Special, dropData);
            var raidData = await GetEventData(Raids, StageType.Raid, dropData);
            var fnData = await GetEventData(Fortnights, StageType.Fortnight, dropData);
            var coloData = await GetEventData(Colos, StageType.Coliseum, dropData);

            var allData = specialData.Concat(raidData).Concat(fnData).Concat(coloData);

            var globalSchedule = await GetSchedule(GlobalSchedule, true, allData);
            var japanSchedule = await GetSchedule(JapanSchedule, false, allData);

            return globalSchedule.Concat(japanSchedule);
        }

        private async Task<IDictionary<string, int>> GetDropData()
        {
            var json = await PerformRequest(Drops);
            var dropSets = JsonConvert.DeserializeObject<JObject>(json);
            var dictionary = new Dictionary<string, int>();
            foreach (var token in dropSets)
            {
                var id = token.Value["id"]?.ToString()?.ToInt32();
                if (id.HasValue)
                {
                    dictionary.Add(token.Key, id.Value);
                }
            }
            return dictionary;
        }

        private async Task<IEnumerable<StageDataParsed>> GetEventData(string endpoint, StageType type, IDictionary<string, int> drops)
        {
            var json = await PerformRequest(endpoint);
            var stageData = JsonConvert.DeserializeObject<JObject>(json);
            var stages = new List<StageDataParsed>();
            foreach (var token in stageData)
            {
                var stage = new StageDataParsed { Type = type, Identifier = token.Key, Representatives = new List<int>() };
                var id = token.Value["linkDB"]?.ToString()?.Split('/').LastOrDefault()?.ToInt32();
                if (id.HasValue)
                {
                    stage.Representatives.Add(id.Value);
                }
                if (!stage.Representatives.Any())
                {
                    var stageDrops = token.Value["drops"];
                    if (stageDrops != null)
                    {
                        foreach (var drop in stageDrops)
                        {
                            var key = drop.ToString();
                            if (drops.ContainsKey(key))
                            {
                                stage.Representatives.Add(drops[key]);
                            }
                        }
                    }
                }
                stages.Add(stage);
            }
            return stages;
        }

        private async Task<IEnumerable<ScheduledEvent>> GetSchedule(string endpoint, bool global, IEnumerable<StageDataParsed> stages)
        {
            var json = await PerformRequest(endpoint);
            var scheduleData = JsonConvert.DeserializeObject<JObject>(json)["weeks"];
            var schedule = new List<ScheduledEvent>();
            var now = DateTimeOffset.Now;
            foreach (var week in scheduleData)
            {
                try
                {
                    var startDate = new DateTimeOffset(now.Year, ParseMonth(week["month"]?.ToString()), week["starting"]?.ToString().ToInt32() ?? 0, 1, 0, 0, TimeSpan.Zero);
                    var endDate = startDate.AddDays(7);
                    var programs = ParsePrograms(week["program"]);
                    var evts = await ParseEvents(programs, startDate, endDate, stages, global);
                    schedule.AddRange(evts);
                }
                catch
                {
                    Debug.WriteLine("Error parsing " + week);
                    // ... eat it.
                }
            }
            return schedule;
        }

        private IEnumerable<ProgramDataParsed> ParsePrograms(JToken program)
        {
            var index = 0;
            var parsed = new List<ProgramDataParsed>();
            foreach (var day in program)
            {
                parsed.AddRange(GetEvents(day["raid"], index, StageType.Raid));
                parsed.AddRange(GetEvents(day["colo"], index, StageType.Coliseum));
                parsed.AddRange(GetEvents(day["fn"], index, StageType.Fortnight));
                parsed.AddRange(GetEvents(day["special"], index, StageType.Special));
                index++;
            }
            return parsed;
        }

        private IEnumerable<ProgramDataParsed> GetEvents(JToken eventType, int offset, StageType type)
        {
            var parsed = new List<ProgramDataParsed>();
            foreach (var item in eventType)
            {
                if (item.ToString() != "none")
                {
                    parsed.Add(new ProgramDataParsed { Identifier = item.ToString(), Offset = offset, Type = type });
                }
            }
            return parsed;
        }

        private async Task<IEnumerable<ScheduledEvent>> ParseEvents(IEnumerable<ProgramDataParsed> programs, DateTimeOffset startDate, DateTimeOffset endDate, IEnumerable<StageDataParsed> stages, bool global)
        {
            var evts = new List<ScheduledEvent>();
            foreach (var program in programs.OrderBy(x => x.Offset))
            {
                if (program.Bypass)
                {
                    continue;
                }
                var stageId = await GetStageId(program.Identifier, program.Type, stages);
                if (stageId == null)
                {
                    continue;
                }
                var length = 0;
                ProgramDataParsed other = null;
                while ((other = programs.FirstOrDefault(x => x.Identifier == program.Identifier && x.Offset == program.Offset + length && !x.Bypass)) != null)
                {
                    other.Bypass = true;
                    length++;
                }
                var evt = new ScheduledEvent
                {
                    StartDate = startDate.AddDays(program.Offset),
                    EndDate = startDate.AddDays(program.Offset + length + 1),
                    Global = global,
                    StageId = stageId.Value
                };
                evts.Add(evt);
            }
            return evts;
        }

        private async Task<int?> GetStageId(string programIdentifier, StageType programType, IEnumerable<StageDataParsed> stages)
        {
            var stage = stages.FirstOrDefault(x => x.Identifier == programIdentifier && x.Type == programType && x.Representatives.Any());
            if (stage == null)
            {
                return null;
            }
            var id = $"{(int)stage.Type:00}{((int?)stage.Representatives.LastOrDefault() ?? 0):0000}{0:00}"?.ToInt32();
            if (Context.Stages.Any(x => x.Id == id))
            {
                return id;
            }
            return null;
        }

        private int ParseMonth(string month)
        {
            switch ((month ?? "").ToLower())
            {
                case "january":
                case "jan":
                    return 1;
                case "february":
                case "feb":
                    return 2;
                case "march":
                case "mar":
                    return 3;
                case "april":
                case "apr":
                    return 4;
                case "may":
                    return 5;
                case "june":
                case "jun":
                    return 6;
                case "july":
                case "jul":
                    return 7;
                case "august":
                case "aug":
                    return 8;
                case "september":
                case "sept":
                case "sep":
                    return 9;
                case "october":
                case "oct":
                    return 10;
                case "november":
                case "nov":
                    return 11;
                case "december":
                case "dec":
                    return 12;
                default:
                    return -1;
            }
        }

        protected override async Task Save(IEnumerable<ScheduledEvent> items)
        {
            Context.ScheduledEvents.Clear();
            await Context.LoopedAddSave(items);
        }

        internal class StageDataParsed
        {
            public StageType Type { get; set; }
            public string Identifier { get; set; }
            public ICollection<int> Representatives { get; set; }
        }

        internal class ProgramDataParsed
        {
            public int Offset { get; set; }
            public StageType Type { get; set; }
            public string Identifier { get; set; }
            public bool Bypass { get; set; }
        }
    }
}
