using System.Collections.Generic;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamStubModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubmittedById { get; set; }
        public string SubmittedByName { get; set; }
        public int Score { get; set; }

        public IEnumerable<TeamUnitStubModel> TeamUnits { get; set; }
    }

    public class TeamDetailModel : TeamStubModel
    {
        public string Description { get; set; }
        public string Credits { get; set; }
        public IEnumerable<TeamSocketStubModel> TeamSockets { get; set; }
    }

    public class TeamEditorModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Credits { get; set; }
        public IEnumerable<TeamSocketEditorModel> TeamSockets { get; set; }
        public IEnumerable<TeamUnitEditorModel> TeamUnits { get; set; }
    }
}
