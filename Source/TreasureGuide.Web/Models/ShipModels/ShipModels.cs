using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.ShipModels
{
    public class ShipStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ShipDetailModel : ShipStubModel
    {
        public string Description { get; set; }
    }

    public class ShipEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
