using System;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Common.Models.ShipModels
{
    public class ShipStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool EventShip { get; set; }
        public bool EventShipActive { get; set; }
    }
}
