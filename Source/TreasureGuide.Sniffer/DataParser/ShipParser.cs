using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer.DataParser
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
                var name = line["name"].ToString();
                var ship = new Ship
                {
                    Id = id,
                    Name = name,
                    Description = line["description"].ToString(),
                    EventShip = IsEventShip(name.ToLower())
                };
                return ship;
            });
            return models;
        }

        private bool IsEventShip(string name)
        {
            return name.Contains("anniversary") || name.Contains("burning whitebeard") || name.Contains("new year") || name.Contains("karasumaru");
        }

        protected override async Task Save(IEnumerable<Ship> ships)
        {
            Context.Ships.Clear();
            await Context.LoopedAddSave(ships);
        }
    }
}
