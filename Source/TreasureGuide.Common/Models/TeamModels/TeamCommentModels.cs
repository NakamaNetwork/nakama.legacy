using System;
using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Common.Models.TeamModels
{
    public class TeamCommentStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Text { get; set; }

        public bool Deleted { get; set; }
        public bool Reported { get; set; }
        public bool CanEdit { get; set; }

        public int MyVote { get; set; }
        public int Score { get; set; }

        public string SubmittedById { get; set; }
        public string SubmittedByName { get; set; }
        public int? SubmittedByUnitId { get; set; }
        public bool SubmittedByIsDonor { get; set; }
        public DateTimeOffset? EditedDate { get; set; }
    }

    public class TeamCommentDetailModel : TeamCommentStubModel { }

    public class TeamCommentEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }
        public int TeamId { get; set; }

        [StringLength(4000)]
        public string Text { get; set; }
    }
}
