using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Common.Validators;

namespace TreasureGuide.Common.Models.TeamModels
{
    public class TeamStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubmittedById { get; set; }
        public string SubmittedByName { get; set; }
        public int? SubmittedByUnitId { get; set; }
        public bool SubmittedByIsDonor { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public int Score { get; set; }
        public bool Global { get; set; }
        public bool F2P { get; set; }
        public bool F2PC { get; set; }
        public bool HasVideos { get; set; }
        public bool HasComments { get; set; }
        public bool HasSupports { get; set; }

        public int ShipId { get; set; }
        public int? StageId { get; set; }
        public int? InvasionId { get; set; }
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
        public bool SubmittedByIsDonor { get; set; }
        public DateTimeOffset? EditedDate { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public int Score { get; set; }
        public int MyVote { get; set; }
        public bool MyBookmark { get; set; }
        public string Guide { get; set; }
        public string Credits { get; set; }
        public bool Global { get; set; }
        public bool F2P { get; set; }
        public bool F2PC { get; set; }
        public int ShipId { get; set; }
        public int? StageId { get; set; }
        public int? InvasionId { get; set; }
        public bool CanEdit { get; set; }
        public bool Deleted { get; set; }
        public bool Reported { get; set; }
        public bool Draft { get; set; }
        public bool HasSupports { get; set; }
        public int Comments { get; set; }

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

        [StringLength(40000, MinimumLength = 20)]
        public string Guide { get; set; }
        public int ShipId { get; set; }
        public int? StageId { get; set; }
        public int? InvasionId { get; set; }
        public bool Deleted { get; set; }
        public bool Draft { get; set; }

        public IEnumerable<TeamSocketEditorModel> TeamSockets { get; set; }

        [EnoughNonSubs(2)]
        [NoDuplicateUnits]
        public IEnumerable<TeamUnitEditorModel> TeamUnits { get; set; }

        [NoDuplicateGenerics]
        public IEnumerable<TeamGenericSlotEditorModel> TeamGenericSlots { get; set; }
    }
}
