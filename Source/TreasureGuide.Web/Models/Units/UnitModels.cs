using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.Units
{
    public class UnitStubModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UnitType Type { get; set; }
        public UnitClassType Class1 { get; set; }
        public UnitClassType? Class2 { get; set; }
        public int? Stars { get; set; }
        public int? Sockets { get; set; }
        public bool Global { get; set; }

        public int LeadTeams { get; set; }
        public int OnTeams { get; set; }
    }
}