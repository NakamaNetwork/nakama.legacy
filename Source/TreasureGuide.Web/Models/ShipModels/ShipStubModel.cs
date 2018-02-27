using System;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.ShipModels
{
    public class ShipStubModel : IIdItem<int>, IEditedDateItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool EventShip { get; set; }
        public bool EventShipActive { get; set; }
        public DateTimeOffset EditedDate { get; set; }
    }
}
