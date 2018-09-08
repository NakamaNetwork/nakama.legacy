namespace TreasureGuide.Common.Models.TeamModels
{
    public class TeamCommentSearchModel : SearchModel
    {
        public bool Deleted { get; set; }
        public bool Reported { get; set; }
    }
}
