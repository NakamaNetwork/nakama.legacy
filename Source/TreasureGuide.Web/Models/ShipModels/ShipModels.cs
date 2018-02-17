using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.ShipModels
{
    public class ShipStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool EventShip { get; set; }
        public bool EventShipActive { get; set; }
    }

    public class ShipDetailModel : ShipStubModel
    {
        public string Description { get; set; }
    }

    public class ShipEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }
    }
}
