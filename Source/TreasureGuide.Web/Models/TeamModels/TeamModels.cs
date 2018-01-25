using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Web.Helpers.Validators;

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
        public bool F2P { get; set; }
        public bool F2PC { get; set; }
        public bool HasVideos { get; set; }

        public int ShipId { get; set; }
        public int? StageId { get; set; }
        public bool Deleted { get; set; }
        public bool Reported { get; set; }
        public bool Draft { get; set; }

        public IEnumerable<TeamUnitStubModel> TeamUnits { get; set; }
        public IEnumerable<TeamGenericSlotStubModel> TeamGenericSlots { get; set; }
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
        public bool F2P { get; set; }
        public bool F2PC { get; set; }
        public int ShipId { get; set; }
        public int? StageId { get; set; }
        public bool CanEdit { get; set; }
        public bool Deleted { get; set; }
        public bool Reported { get; set; }
        public bool Draft { get; set; }

        public IEnumerable<TeamUnitDetailModel> TeamUnits { get; set; }
        public IEnumerable<TeamGenericSlotDetailModel> TeamGenericSlots { get; set; }
        public IEnumerable<TeamSocketStubModel> TeamSockets { get; set; }
        public IEnumerable<TeamVideoModel> TeamVideos { get; set; }
    }

    [NotTooManyUnits(5)]
    [EnoughUnits(4)]
    public class TeamEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }

        [StringLength(250, MinimumLength = 10)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Credits { get; set; }

        [StringLength(40000)]
        public string Guide { get; set; }
        public int ShipId { get; set; }
        public int? StageId { get; set; }
        public bool Deleted { get; set; }
        public bool Draft { get; set; }

        public IEnumerable<TeamSocketEditorModel> TeamSockets { get; set; }

        [EnoughNonSubs(2)]
        [OnlyOneNonSub]
        [NoDuplicateUnits]
        public IEnumerable<TeamUnitEditorModel> TeamUnits { get; set; }

        [OnlyOneNonSub]
        [NoDuplicateGenerics]
        public IEnumerable<TeamGenericSlotEditorModel> TeamGenericSlots { get; set; }
    }
}
