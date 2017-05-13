using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;

namespace TreasureGuide.Sniffer.Parsers
{
    public class ShipParser : TreasureParser<IEnumerable<Ship>>
    {
        private const string OptcDbUnitData = "https://raw.githubusercontent.com/optc-db/optc-db.github.io/master/common/data/ships.js";

        public ShipParser(TreasureEntities context) : base(context, OptcDbUnitData)
        {
        }

        protected override IEnumerable<Ship> ConvertData(string trimmed)
        {
            var arrays = JsonConvert.DeserializeObject<JArray>(trimmed);
            var models = arrays.Select((line, index) =>
            {
                var id = index;
                var ship = new Ship
                {
                    Id = id,
                    Name = line["name"].ToString()
                };
                return ship;
            });
            return models;
        }

        protected override async Task Save(IEnumerable<Ship> ships)
        {
            Context.Ships.Clear();
            foreach (var ship in ships)
            {
                Context.Ships.Add(ship);
            }
            await Context.SaveChangesAsync();
        }
    }
}
