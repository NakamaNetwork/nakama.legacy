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
    public class UnitParser : TreasureParser<IEnumerable<ParsedUnitModel>>
    {
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/units.js";

        public UnitParser(TreasureEntities context) : base(context, OptcDbUnitData)
        {
        }

        protected override IEnumerable<ParsedUnitModel> ConvertData(string trimmed)
        {
            var arrays = JsonConvert.DeserializeObject<object[][]>(trimmed);
            var models = arrays.Select((line, index) =>
            {
                var id = index + 1;
                var classData = line[2]?.ToString();
                var unit = new Unit
                {
                    Id = id,
                    Name = line[0] as string,
                    Type = (line[1] as string).ToUnitType(),
                    // Classes parsed later
                    Stars = (line[3]?.ToString())?.ToByte(),
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
                    GrowthRate = (line[15]?.ToString()).ToDecimal(),
                };
                return new ParsedUnitModel
                {
                    Unit = unit,
                    UnitClasses = (classData.Contains("[") ? JsonConvert.DeserializeObject<string[]>(classData) : new[] { classData })
                        .Select(type => type.ToUnitClass())
                        .Where(x => x != UnitClassType.Unknown)
                        .Select(type => new UnitClass
                        {
                            Unit = unit,
                            Class = type
                        })
                };
            });
            return models;
        }

        protected override async Task Save(IEnumerable<ParsedUnitModel> items)
        {
            Context.Units.Clear();
            Context.UnitClasses.Clear();
            foreach (var unit in items)
            {
                Context.Units.Add(unit.Unit);
                Context.UnitClasses.AddRange(unit.UnitClasses);
            }
            await Context.SaveChangesAsync();
        }
    }

    public class ParsedUnitModel
    {
        public Unit Unit { get; set; }
        public IEnumerable<UnitClass> UnitClasses { get; set; }
    }
}
