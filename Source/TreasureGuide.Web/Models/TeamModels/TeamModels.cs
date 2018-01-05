using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubmittedById { get; set; }
        public string SubmittedByName { get; set; }
        public int? SubmittedByUnitId { get; set; }
        public DateTimeOffset EditedDate { get; set; }
        public int Score { get; set; }
        public bool Global { get; set; }
        public int ShipId { get; set; }
        public int? StageId { get; set; }

        public IEnumerable<TeamUnitStubModel> TeamUnits { get; set; }
    }

    public class TeamDetailModel : IIdItem<int>, ICanEdit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubmittedById { get; set; }
        public string SubmittedByName { get; set; }
        public int? SubmittedByUnitId { get; set; }
        public DateTimeOffset EditedDate { get; set; }
        public int Score { get; set; }
        public int MyVote { get; set; }
        public string Guide { get; set; }
        public string Credits { get; set; }
        public bool Global { get; set; }
        public int ShipId { get; set; }
        public int? StageId { get; set; }
        public bool CanEdit { get; set; }

        public IEnumerable<TeamUnitDetailModel> TeamUnits { get; set; }
        public IEnumerable<TeamSocketStubModel> TeamSockets { get; set; }
    }

    public class TeamEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }

        [StringLength(250, MinimumLength = 10)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Credits { get; set; }

        [StringLength(10000)]
        public string Guide { get; set; }
        public int ShipId { get; set; }
        public int? StageId { get; set; }

        public IEnumerable<TeamSocketEditorModel> TeamSockets { get; set; }
        public IEnumerable<TeamUnitEditorModel> TeamUnits { get; set; }
    }
}
