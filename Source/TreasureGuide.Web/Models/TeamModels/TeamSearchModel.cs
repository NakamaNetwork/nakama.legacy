namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamSearchModel : SearchModel
    {
        public string Term { get; set; }
        public int? LeaderId { get; set; }
        public int? StageId { get; set; }

        public bool MyBox { get; set; }
        public bool Global { get; set; }
    }
}
