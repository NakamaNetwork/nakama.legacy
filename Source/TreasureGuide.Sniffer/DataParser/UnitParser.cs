using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jurassic;
using Newtonsoft.Json;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer.DataParser
{
    public class UnitParser : TreasureParser<IEnumerable<Unit>>
    {
        private static readonly Regex NumberRegex = new Regex("\\d+?");
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/units.js";

        public UnitParser(TreasureEntities context) : base(context, OptcDbUnitData)
        {
        }

        protected override string TrimData(string input)
        {
            input = input.Replace("window.", "");
            var engine = new ScriptEngine();
            engine.Evaluate(input);
            engine.Evaluate("jsonUnits = JSON.stringify(units);");
            var json = engine.GetGlobalValue<string>("jsonUnits");
            return json;
        }

        protected override IEnumerable<Unit> ConvertData(string trimmed)
        {
            trimmed = trimmed.Replace("'6+'", "\"6+\"").Replace("'5+'", "\"5+\"");
            var arrays = JsonConvert.DeserializeObject<object[][]>(trimmed);
            var models = arrays.Select((line, index) =>
            {
                var id = index + 1;
                var classData = line[2]?.ToString();
                var parsedClasses = classData.Replace("[", "").Replace("]", "").Replace("\"", "").Split(',').Select(type => type.ToUnitClass()).Distinct().Where(x => x != UnitClass.Unknown);
                var unitClass = parsedClasses.Aggregate(UnitClass.Unknown, (current, parsed) => current | parsed);
                var unit = new Unit
                {
                    Id = id,
                    Name = line[0] as string,
                    Type = (line[1] as string).ToUnitType(),
                    Class = unitClass,
                    // Classes parsed later
                    Stars = ParseStars(line[3]?.ToString()),
                    Cost = (line[4]?.ToString())?.ToByte(),
                    Combo = (line[5]?.ToString())?.ToByte(),
                    Sockets = (line[6]?.ToString())?.ToByte(),
                    MaxLevel = (line[7]?.ToString())?.ToByte(),
                    EXPtoMax = (line[8]?.ToString())?.ToInt32(),
                    MinHP = (line[9]?.ToString()).ToInt16(),
                    MinATK = (line[10]?.ToString()).ToInt16(),
                    MinRCV = (line[11]?.ToString()).ToInt16(),
                    MaxHP = (line[12]?.ToString()).ToInt16(),
                    MaxATK = (line[13]?.ToString()).ToInt16(),
                    MaxRCV = (line[14]?.ToString()).ToInt16(),
                    GrowthRate = (line[15]?.ToString()).ToDecimal()
                };
                return unit;
            });
            return models.Where(x => !String.IsNullOrWhiteSpace(x.Name) &&
            !(x.Id >= 5000 && x.Name.Contains("[Dual Unit]")) // Dual Unit Filter
            );
        }

        private decimal? ParseStars(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            var number = NumberRegex.Match(value);
            if (number.Success)
            {
                var result = number.Value.ToDecimal();
                if (value.Contains("+"))
                {
                    result += (decimal?)0.5d;
                }
                return result;
            }
            return null;
        }

        protected override async Task Save(IEnumerable<Unit> units)
        {
            Context.Units.Clear();
            await Context.LoopedAddSave(units);
        }
    }
}
