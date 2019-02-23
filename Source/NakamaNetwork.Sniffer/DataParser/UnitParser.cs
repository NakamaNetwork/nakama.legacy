using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NakamaNetwork.Entities.EnumTypes;
using NakamaNetwork.Entities.Helpers;
using NakamaNetwork.Entities.Models;
using NakamaNetwork.Sniffer.Helpers;
using Newtonsoft.Json;

namespace NakamaNetwork.Sniffer.DataParser
{
    public class UnitParser : TreasureParser<IEnumerable<Unit>>
    {
        private static readonly Regex NumberRegex = new Regex("\\d+?");
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/units.js";

        public UnitParser(NakamaNetworkContext context) : base(context, OptcDbUnitData)
        {
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
                    ExptoMax = (line[8]?.ToString())?.ToInt32(),
                    MinHp = (line[9]?.ToString()).ToInt16(),
                    MinAtk = (line[10]?.ToString()).ToInt16(),
                    MinRcv = (line[11]?.ToString()).ToInt16(),
                    MaxHp = (line[12]?.ToString()).ToInt16(),
                    MaxAtk = (line[13]?.ToString()).ToInt16(),
                    MaxRcv = (line[14]?.ToString()).ToInt16(),
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
