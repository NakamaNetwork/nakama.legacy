namespace TreasureGuide.Common.Models.TeamModels
{
    public class TeamCommentSearchModel : SearchModel
    {
        public int TeamId { get; set; }
        public bool Deleted { get; set; }
        public bool Reported { get; set; }
    }
}
