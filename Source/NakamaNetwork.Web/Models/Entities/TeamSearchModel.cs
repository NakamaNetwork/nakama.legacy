using NakamaNetwork.Entities.EnumTypes;

namespace TreasureGuide.Common.Models.TeamModels
{
    public class TeamSearchModel : SearchModel
    {
        public string Term { get; set; }
        public string SubmittedBy { get; set; }
        public int? LeaderId { get; set; }
        public bool NoHelp { get; set; }
        public int? StageId { get; set; }
        public int? InvasionId { get; set; }

        public int? BoxId { get; set; }
        public bool Global { get; set; }
        public FreeToPlayStatus FreeToPlay { get; set; }
        public UnitClass Classes { get; set; }
        public UnitType Types { get; set; }

        public bool Deleted { get; set; }
        public bool Draft { get; set; }
        public bool Reported { get; set; }
        public bool Bookmark { get; set; }
        public bool ExcludeEventShips { get; set; }
    }

    public enum FreeToPlayStatus
    {
        None,
        All,
        Crew
    }
}
